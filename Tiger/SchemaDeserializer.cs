﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tiger;

public class SchemaDeserializer : Strategy.StrategistSingleton<SchemaDeserializer>
{
    // Stores both SchemaStruct and SchemaType sizes
    private ConcurrentDictionary<Type, int> _schemaSerializedSizeMap;
    // First maps the struct schema type, then the field name
    private ConcurrentDictionary<Type, Dictionary<string, int>> _schemaFieldOffsetMap;

    public SchemaDeserializer(TigerStrategy strategy) : base(strategy)
    {
        _schemaSerializedSizeMap = new ConcurrentDictionary<Type, int>();
        _schemaFieldOffsetMap = new ConcurrentDictionary<Type, Dictionary<string, int>>();

        FillSchemaCaches();
    }

    private void FillSchemaCaches()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());

        Parallel.ForEach(types, type =>
        {
            SchemaStructAttribute? schemaStructAttribute = GetAttribute<SchemaStructAttribute>(type);
            if (schemaStructAttribute != null)
            {
                _schemaSerializedSizeMap.TryAdd(type, schemaStructAttribute.SerializedSize);
                _schemaFieldOffsetMap.TryAdd(type, new Dictionary<string, int>());

                foreach (FieldInfo fieldInfo in type.GetFields())
                {
                    SchemaFieldAttribute? schemaFieldAttribute = GetAttribute<SchemaFieldAttribute>(fieldInfo);
                    if (schemaFieldAttribute != null)
                    {
                        _schemaFieldOffsetMap[type].TryAdd(fieldInfo.Name, schemaFieldAttribute.Offset);
                    }
                    // else
                    // {
                    //     FieldOffsetAttribute? fieldOffsetAttribute = (FieldOffsetAttribute?)fieldInfo.GetCustomAttribute(typeof(FieldOffsetAttribute), false);
                    //     if (fieldOffsetAttribute != null)
                    //     {
                    //         _schemaFieldOffsetMap[type].TryAdd(fieldInfo.Name, fieldOffsetAttribute.Value);
                    //     }
                    // }

                    if (!fieldInfo.FieldType.IsGenericType && !_schemaSerializedSizeMap.ContainsKey(fieldInfo.FieldType))
                    {
                        _schemaSerializedSizeMap.TryAdd(fieldInfo.FieldType, Marshal.SizeOf(fieldInfo.FieldType));
                        continue;
                    }
                }

                return;
            }

            SchemaTypeAttribute? schemaTypeAttribute = (SchemaTypeAttribute?)type.GetCustomAttribute(typeof(SchemaTypeAttribute), true);
            if (schemaTypeAttribute != null)
            {
                _schemaSerializedSizeMap.TryAdd(type, schemaTypeAttribute.SerializedSize);
                return;
            }
        });
    }

    public T DeserializeSchema<T>(TigerReader reader) where T : struct
    {
        return DeserializeSchema(reader, typeof(T));
    }

    public dynamic DeserializeSchema(TigerReader reader, Type schemaType)
    {
        object resource = Activator.CreateInstance(schemaType);

        FieldInfo[] fields = schemaType.GetFields();
        long startOffset = reader.Position;
        long fieldOffset = 0;
        foreach (FieldInfo fieldInfo in fields)
        {
            if (GetSchemaFieldOffset(schemaType, fieldInfo, out int offset))
            {
                fieldOffset = offset;
            }

            reader.Seek(startOffset + fieldOffset, SeekOrigin.Begin);

            long fieldSize = -1;
            dynamic? fieldValue;
            if (IsTigerDeserializeType(fieldInfo))
            {
                fieldValue = DeserializeTigerType(reader, fieldInfo.FieldType);
                fieldSize = GetSchemaTypeSize(fieldInfo.FieldType);
            }
            else if (IsTigerFile64Type(fieldInfo))
            {
                uint u32 = reader.ReadUInt32();
                int bIs32Bit = reader.ReadInt32();
                ulong u64 = reader.ReadUInt64();
                if (bIs32Bit == 1)
                {
                    fieldValue = FileResourcer.Get().GetFile(fieldInfo.FieldType, new FileHash(u32));
                }
                else if (bIs32Bit == 0)
                {
                    fieldValue = FileResourcer.Get().GetFile(fieldInfo.FieldType, new File64Hash(u64));
                }
                else
                {
                    throw new Exception("Invalid bIs32Bit value");
                }

                fieldSize = GetSchemaTypeSize(fieldInfo.FieldType);
            }
            else if (IsTigerFileType(fieldInfo))
            {
                FileHash fileHash = new FileHash(reader.ReadUInt32());
                fieldValue = FileResourcer.Get().GetFile(fieldInfo.FieldType, fileHash);
                fieldSize = GetSchemaTypeSize(fieldInfo.FieldType);
            }
            else
            {
                fieldValue = DeserializeMarshalType(reader, fieldInfo.FieldType);
                fieldSize = Marshal.SizeOf(fieldInfo.FieldType);
            }
            fieldInfo.SetValue(resource, fieldValue); // todo check if this set is required, might be a reference type?

            if (fieldSize == -1)
            {
                throw new Exception($"Failed to get field size for field {fieldInfo.Name} of type {fieldInfo.FieldType}");
            }
            fieldOffset += fieldSize;
        }


        return resource;
    }

    private bool IsTigerDeserializeType(FieldInfo fieldInfo)
    {
        return fieldInfo.FieldType.FindInterfaces((type, _) => type == typeof(ITigerDeserialize), null).Length > 0;
    }

    private bool IsTigerFileType(FieldInfo fieldInfo)
    {
        return fieldInfo.FieldType.IsSubclassOf(typeof(TigerFile));
    }

    private bool IsTigerFile64Type(FieldInfo fieldInfo)
    {
        return fieldInfo.FieldType.IsSubclassOf(typeof(Tag64<>));
    }

    private dynamic DeserializeTigerType(TigerReader reader, Type fieldType)
    {
        ITigerDeserialize? fieldValue = (ITigerDeserialize?)Activator.CreateInstance(fieldType);
        if (fieldValue == null)
        {
            throw new Exception($"Failed to create schema field instance of type {fieldType}");
        }

        fieldValue.Deserialize(reader);
        return fieldValue;
    }

    private dynamic DeserializeMarshalType(TigerReader reader, Type fieldType)
    {
        return reader.ReadType(fieldType);
    }

    public int GetSchemaStructSize<T>() where T : struct
    {
        return GetSchemaStructSize(typeof(T));
    }

    private int GetSchemaStructSize(Type type)
    {
        if (_schemaSerializedSizeMap.TryGetValue(type, out int size))
        {
            return size;
        }
        else
        {
            return Marshal.SizeOf(type);
        }
    }

    private int GetSchemaTypeSize(Type type)
    {
        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }
        if (_schemaSerializedSizeMap.TryGetValue(type, out int size))
        {
            return size;
        }
        else
        {
            throw new Exception($"Failed to get schema type size for type {type} as it has no schema type attribute");
        }
    }

    private bool GetSchemaFieldOffset(Type schemaType, FieldInfo fieldInfo, out int offset)
    {
        if (!_schemaFieldOffsetMap.ContainsKey(schemaType))
        {
            offset = 0;
            return false;
        }
        return _schemaFieldOffsetMap[schemaType].TryGetValue(fieldInfo.Name, out offset);
        //
        // offset = 0;
        // SchemaFieldAttribute? attribute = GetAttribute<SchemaFieldAttribute>(fieldInfo);
        // if (attribute == null)
        // {
        //     return GetFieldOffset(fieldInfo, out offset);
        // }
        // offset = attribute.Offset;
        // return true;
    }

    // private bool GetFieldOffset(FieldInfo fieldInfo, out int offset)
    // {
    //     offset = 0;
    //     FieldOffsetAttribute? attribute = (FieldOffsetAttribute?)fieldInfo.GetCustomAttribute(typeof(FieldOffsetAttribute), false);
    //     if (attribute == null)
    //     {
    //         return false;
    //     }
    //     offset = attribute.Value;
    //     return true;
    // }

    private T? GetAttribute<T>(ICustomAttributeProvider var) where T : StrategyAttribute
    {
        T[] attributes = var.GetCustomAttributes(typeof(T), false).Cast<T>().ToArray();
        if (!attributes.Any())
        {
            // we still want to be able to get size of non-schema types
            return null;
        }
        // if there's only one attribute, presume it applies to every strategy
        if (attributes.Length == 1)
        {
            return attributes.First();
        }

        // otherwise we need to find the one that matches the current strategy
        Dictionary<TigerStrategy, T> map = attributes.ToDictionary(a => a.Strategy, a => a);
        map.GetFullStrategyMap();
        if (map.TryGetValue(_strategy, out T attribute))
        {
            return attribute;
        }
        else
        {
            throw new Exception($"Failed to get schema struct size for type {var} as it has multiple schema struct attributes but none match the current strategy {_strategy}");
        }
    }
}
