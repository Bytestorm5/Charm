﻿using System.Runtime.InteropServices;
using DirectXTexNet;
using Field;
using Microsoft.VisualBasic.FileIO;
using Tiger.Audio;
using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

namespace Tiger.Schema.Entity;

[StructLayout(LayoutKind.Sequential, Size = 0x98)]
public struct D2Class_D89A8080  // Entity
{
    public long FileSize;
    public DynamicArray<D2Class_CD9A8080> EntityResources;
    public DynamicArray<D2Class_8F9A8080> Unk18;
    public long Zeros28;
    public long Zeros30;
    public TigerHash Unk38;
    public int Zeros3C;
    public DynamicArray<D2Class_F09A8080> Unk40;
    public DynamicArray<D2Class_ED9A8080> Unk50;
    public DynamicArray<D2Class_EB9A8080> Unk60;
    public DynamicArray<D2Class_06008080> Unk70;
    public TigerHash Unk80;
    public TigerHash Unk84;
    public TigerHash Unk88;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_F09A8080
{
    public TigerHash Unk00;
    public ushort Unk04;
    public ushort Unk06;
}

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public struct D2Class_ED9A8080
{
    public short Unk00;
    public short Unk02;
    public short Unk04;
    public short Unk06;
    public int Unk08;  // some pointer
    // public TagTypeHash Unk0C;  // the class of the final target resource
    // [DestinyField(FieldType.ResourceInTagWeird)]
    // public dynamic? Resource10;  // non-standard resource in tag, the resource type is actually the one before and its like a double-pointer thing. means nothing to me so wont parse these kinds.
    // [SchemaField(0x20), DestinyField(FieldType.FileHash)]
    // public Tag Unk20;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_EB9A8080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Resource00;

    public long Unk10;
}

[StructLayout(LayoutKind.Sequential, Size = 2)]
public struct D2Class_06008080
{
    public short Unk0;
}

[StructLayout(LayoutKind.Sequential, Size = 0xC)]
public struct D2Class_CD9A8080  // entity resource entry
{
    public EntityResource ResourceHash;
}

[StructLayout(LayoutKind.Sequential, Size = 0xA0)]
public struct D2Class_069B8080  // Entity resource
{
    public long FileSize;
    public ResourcePointer Unk08;
    public ResourcePointer Unk10;
    public ResourcePointer Unk18;
    // public Table ResourceTable20;
    // public Table ResourceTable30;
    [SchemaField(0x40)]
    public DynamicArray<D2Class_7C908080> ResourceTable40;
    // public Table ResourceTable50;
    [SchemaField(0x60)]
    public DynamicArray<D2Class_6E908080> ResourceTable60;
    // public Table ResourceTable70;
    [SchemaField(0x80)]
    public Tag UnkHash80;
    public Tag UnkHash84;  // 819A8080
    // Rest is unknown
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_7C908080
{
    [DestinyField(FieldType.ResourcePointerWithClass)]
    public Resource ResourcePointer00;
    public TigerHash ResourceClassHash08;
    public short Unk0C;
    public short Unk0E;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_6E908080
{
    [DestinyField(FieldType.RelativePointer)]
    public long RelativePointer00;
}

/*
 * The external material map provides the mapping of external material index -> material tag
 * could be these external materials are dynamic themselves - we'll extract them all but select the first
 */
[StructLayout(LayoutKind.Sequential, Size = 0x450)]
public struct D2Class_8F6D8080
{
    [SchemaField(0x224)]
    public EntityModel Model;
    [SchemaField(0x310)]
    public Tag<D2Class_1C6E8080> TexturePlates;
    [SchemaField(0x3C0)]
    public DynamicArray<D2Class_976D8080> ExternalMaterialsMap;
    [SchemaField(0x400)]
    public DynamicArray<D2Class_14008080> ExternalMaterials;
}

#region Texture Plates

public class TexturePlate : Tag<D2Class_919E8080>
{
    public TexturePlate(FileHash hash) : base(hash)
    {
    }

    public ScratchImage? MakePlatedTexture()
    {
        var dimension = GetPlateDimensions();
        if (dimension.X == 0)
        {
            return null;
        }

        bool bSrgb = _tag.PlateTransforms[0].Texture.IsSrgb();
        ScratchImage outputPlate = TexHelper.Instance.Initialize2D(bSrgb ? DXGI_FORMAT.B8G8R8A8_UNORM_SRGB : DXGI_FORMAT.B8G8R8A8_UNORM, dimension.X, dimension.Y, 1, 0, 0);

        foreach (var transform in _tag.PlateTransforms)
        {
            ScratchImage original = transform.Texture.GetScratchImage();
            ScratchImage resizedOriginal = original.Resize(transform.Scale.X, transform.Scale.Y, 0);
            TexHelper.Instance.CopyRectangle(resizedOriginal.GetImage(0, 0, 0), 0, 0, transform.Scale.X, transform.Scale.Y, outputPlate.GetImage(0, 0, 0), bSrgb ? TEX_FILTER_FLAGS.SRGB : 0, transform.Translation.X, transform.Translation.Y);
            original.Dispose();
            resizedOriginal.Dispose();
        }

        return outputPlate;
    }

    public void SavePlatedTexture(string savePath)
    {
        var simg = MakePlatedTexture();
        if (simg != null)
        {
            Texture.SavetoFile(savePath, simg);
        }
    }

    public IntVector2 GetPlateDimensions()
    {
        int maxDimension = 0;  // plate must be square
        foreach (var transform in _tag.PlateTransforms)
        {
            if (transform.Translation.X + transform.Scale.X > maxDimension)
            {
                maxDimension = transform.Translation.X + transform.Scale.X;
            }
            if (transform.Translation.Y + transform.Scale.Y > maxDimension)
            {
                maxDimension = transform.Translation.Y + transform.Scale.Y;
            }
        }

        // Find power of two that fits this dimension, ie round up the exponent to nearest integer
        maxDimension = (int)Math.Pow(2, Math.Ceiling(Math.Log2(maxDimension)));

        return new IntVector2(maxDimension, maxDimension);
    }
}

/// <summary>
/// Texture plate header that stores all the texture plates used for the EntityModel.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 0x38)]
public struct D2Class_1C6E8080
{
    public long FileSize;
    [SchemaField(0x18)]
    public byte Unk18;
    public byte Unk19;
    public short Unk1A;
    public int Unk1C;
    public int Unk20;
    public int Unk24;
    public TexturePlate AlbedoPlate;
    public TexturePlate NormalPlate;
    public TexturePlate GStackPlate;
    public TexturePlate DyemapPlate;
}

/// <summary>
/// Texture plate that stores the data for placing textures on a canvas.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_919E8080
{
    public long FileSize;
    [SchemaField(0x10)]
    public DynamicArray<D2Class_939E8080> PlateTransforms;
}

[StructLayout(LayoutKind.Sequential, Size = 0x14)]
public struct D2Class_939E8080
{
    public Texture Texture;
    public IntVector2 Translation;
    public IntVector2 Scale;
}

#endregion

[StructLayout(LayoutKind.Sequential, Size = 0xC)]
public struct D2Class_976D8080
{
    public int MaterialCount;
    public int MaterialStartIndex;
    public int Unk08;  // maybe some kind of LOD or dynamic marker
}

[StructLayout(LayoutKind.Sequential, Size = 0x4)]
public struct D2Class_14008080
{
    public Material Material;
}

[StructLayout(LayoutKind.Sequential, Size = 0x38)]
public struct D2Class_8F9A8080
{
    // public InlineGlobalPointer Unk0;
    [SchemaField(0x10)]
    public TigerHash Unk10;
    // public InlineGlobalPointer Unk18;
    [SchemaField(0x28)]
    public TigerHash Unk28;
}

[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_9F9A8080
{

}

[StructLayout(LayoutKind.Sequential, Size = 0x40)]
public struct D2Class_5F6E8080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x2E0)]
public struct D2Class_8A6D8080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_F39A8080
{

}

[StructLayout(LayoutKind.Sequential, Size = 0x100)]
public struct D2Class_DD818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    [SchemaField(0x30)]
    public DynamicArray<D2Class_DC818080> Unk30;
    public DynamicArray<D2Class_40868080> Unk40;
}

[StructLayout(LayoutKind.Sequential, Size = 0x40)]
public struct D2Class_DC818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    public ResourcePointer Unk10;
    [SchemaField(0x20)]
    public DynamicArray<D2Class_4F9F8080> Unk20;
}

[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_4F9F8080
{
    public Tiger.Schema.Vector4 Rotation;
    public Tiger.Schema.Vector4 Translation;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_40868080
{
    public ushort Unk00;
    public ushort Unk02;
    public uint Unk04;
}

[StructLayout(LayoutKind.Sequential, Size = 0x108)]
public struct D2Class_DE818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    [SchemaField(0x48)]
    public ResourcePointer Unk48;
    public Tag Unk50;  // 239B8080
    [SchemaField(0x68)]
    public ResourcePointer Unk68;
    public Tag Unk70;  // 239B8080
    [SchemaField(0x88)]
    public TigerHash Unk88;
    public TigerHash Unk8C;
    public DynamicArray<D2Class_42868080> NodeHierarchy;
    public DynamicArray<D2Class_4F9F8080> DefaultObjectSpaceTransforms;
    public DynamicArray<D2Class_4F9F8080> DefaultInverseObjectSpaceTransforms;
    public DynamicArray<D2Class_06008080> RangeIndexMap;
    public DynamicArray<D2Class_06008080> InnerIndexMap;
    public Vector4 UnkE0;
    public DynamicArray<D2Class_E1818080> UnkF0; // lod distance?
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_42868080
{
    public TigerHash NodeHash;
    public int ParentNodeIndex;
    public int FirstChildNodeIndex;
    public int NextSiblingNodeIndex;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_E1818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    public long Unk10;
}




[StructLayout(LayoutKind.Sequential, Size = 0xA0)]
public struct D2Class_076F8080  // Entity model
{
    public long FileSize;
    [SchemaField(0x10)]
    public DynamicArray<D2Class_C56E8080> Meshes;
    [SchemaField(0x20)]
    public Vector4 Unk20;
    public long Unk30;
    [SchemaField(0x38)]
    public long UnkFlags38;
    [SchemaField(0x40)]
    public long Unk40;
    [SchemaField(0x50)]
    public Vector4 ModelScale;
    public Vector4 ModelTranslation;
    public Vector2 TexcoordScale;
    public Vector2 TexcoordTranslation;
    public Vector4 Unk80;
    public TigerHash Unk90;
    public TigerHash Unk94;
}

[StructLayout(LayoutKind.Sequential, Size = 0x80)]
public struct D2Class_C56E8080
{
    public VertexHeader Vertices1;  // vert file 1 (positions)
    public VertexHeader Vertices2;  // vert file 2 (texcoords/normals)
    public VertexHeader OldWeights;  // old weights
    public TigerHash Unk0C;  // nothing ever
    public IndexHeader Indices;  // indices
    public VertexHeader VertexColour;  // vertex colour
    public VertexHeader SinglePassSkinningBuffer;  // single pass skinning buffer
    public int Zeros1C;
    public DynamicArray<D2Class_CB6E8080> Parts;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
    public short[] StagePartOffsets;
}

[StructLayout(LayoutKind.Sequential, Size = 0x24)]
public struct D2Class_CB6E8080  // TODO use DCG to figure out what this is
{
    public Schema.Material Material;  // AA6D8080
    public short VariantShaderIndex;  // variant_shader_index
    public short PrimitiveType;
    public uint IndexOffset;
    public uint IndexCount;
    public uint Unk10;  // might be number of strips?
    public short ExternalIdentifier;  // external_identifier
    public short Unk16;  // some kind of index
    public int Flags;
    public byte GearDyeChangeColorIndex;   // sbyte gear_dye_change_color_index
    public ELodCategory LodCategory;
    public byte Unk1E;
    public byte LodRun;  // lod_run
    public int Unk20; // variant_shader_index?
}

[StructLayout(LayoutKind.Sequential, Size = 0x320)]
public struct D2Class_5B6D8080
{
    // Full of relative pointer shit
    // Tables start at 0x1f0
    [SchemaField(0x210)]
    public DynamicArray<D2Class_0B008080> Unk210;
    [SchemaField(0x220)]
    public DynamicArray<D2Class_D99E8080> Unk220;
    // there are more tables
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct D2Class_0B008080
{
    public uint Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_D99E8080
{
    public DynamicArray<D2Class_0B008080> Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x480)]
public struct D2Class_6C6D8080
{
    [SchemaField(0x38)]
    public DynamicArray<D2Class_F79A8080> Unk38;
    [SchemaField(0x224)]
    public EntityModel PhysicsModel;
    [SchemaField(0x2E8)]
    public DynamicArray<D2Class_9E958080> Unk2E8;
    // there are more tables
    [SchemaField(0x470)]
    public Tag Unk470;  // 606D8080
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_F79A8080
{
    public ulong Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct D2Class_9E958080
{
    public uint Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x70)]
public struct D2Class_668B8080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    [SchemaField(0x30)]
    public DynamicArray<D2Class_628B8080> Unk30;
}

[StructLayout(LayoutKind.Sequential, Size = 0x30)]
public struct D2Class_628B8080
{
    public Vector4 Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x140)]
public struct D2Class_5F8B8080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    [SchemaField(0xA8)]
    public DynamicArray<D2Class_5F8C8080> UnkA8;
    public DynamicArray<D2Class_568C8080> UnkB8;
    public DynamicArray<D2Class_4F9F8080> UnkC8;
    public DynamicArray<D2Class_06008080> UnkD8;
    [SchemaField(0xF0)]
    public DynamicArray<D2Class_06008080> UnkF0;
    public DynamicArray<D2Class_06008080> Unk100;
    [SchemaField(0x128)]
    public DynamicArray<D2Class_DA8B8080> Unk128;
    public int Unk138;
    public short Unk13C;
    public short Unk13E;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_5F8C8080
{
    public TigerHash Unk00;
    public int Unk04;
}

[StructLayout(LayoutKind.Sequential, Size = 0x34)]
public struct D2Class_568C8080
{
    public TigerHash Unk00;
    public short Unk04;
    public short Unk06;
    public short Unk08;
    public short Unk0A;
    public short Unk0C;
    public short Unk0E;
    public float Unk10;
    public int Unk14;

    public short Unk18;
    public short Unk1A;
    public short Unk1C;
    public short Unk1E;
    public short Unk20;
    public short Unk22;
    public float Unk24;
    public int Unk28;

    public short Unk2C;
    public short Unk2E;
    public sbyte Unk30;
    public sbyte Unk31;
    public sbyte Unk32;
    public sbyte Unk33;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_DA8B8080
{
    public TigerHash Unk00;
    public int Unk04;
}

[StructLayout(LayoutKind.Sequential, Size = 0x830)]
public struct D2Class_13268080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    // lots of array stuff
}

[StructLayout(LayoutKind.Sequential, Size = 0x830)]
public struct D2Class_F8258080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    // lots of array stuff
    [SchemaField(0xA8), MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public Tag[] UnkA8;
}

[StructLayout(LayoutKind.Sequential, Size = 0xBA0)]
public struct D2Class_41268080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
    // lots of array stuff
    [SchemaField(0x1E0)]
    public DynamicArray<D2Class_0B008080> Unk1E0;
    public DynamicArray<D2Class_0F008080> Unk1F0;
    public DynamicArray<D2Class_90008080> Unk200;

    [SchemaField(0x240)]
    public DynamicArray<D2Class_0B008080> Unk240;
    public DynamicArray<D2Class_0F008080> Unk250;
    public DynamicArray<D2Class_90008080> Unk260;

    [SchemaField(0x5F8)]
    public DynamicArray<D2Class_86268080> Unk5F8;
    [SchemaField(0x658)]
    public DynamicArray<D2Class_86268080> Unk658;
    [SchemaField(0x6B8)]
    public DynamicArray<D2Class_86268080> Unk6B8;
    [SchemaField(0x718)]
    public DynamicArray<D2Class_86268080> Unk718;
    [SchemaField(0x778)]
    public DynamicArray<D2Class_86268080> Unk778;
    [SchemaField(0x7D8)]
    public DynamicArray<D2Class_86268080> Unk7D8;
    [SchemaField(0x838)]
    public DynamicArray<D2Class_86268080> Unk838;
    [SchemaField(0x898)]
    public DynamicArray<D2Class_86268080> Unk898;
    [SchemaField(0x8F8)]
    public DynamicArray<D2Class_86268080> Unk8F8;
    [SchemaField(0x958)]
    public DynamicArray<D2Class_86268080> Unk958;
    [SchemaField(0x9B8)]
    public DynamicArray<D2Class_86268080> Unk9B8;
    [SchemaField(0xA18)]
    public DynamicArray<D2Class_86268080> UnkA18;
    [SchemaField(0xA78)]
    public DynamicArray<D2Class_86268080> UnkA78;
    [SchemaField(0xAD8)]
    public DynamicArray<D2Class_86268080> UnkAD8;
    [SchemaField(0xB38)]
    public DynamicArray<D2Class_86268080> UnkB38;

    [SchemaField(0xB70)]
    public DynamicArray<D2Class_72268080> Unk6E8;
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct D2Class_0F008080
{
    public float Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_90008080
{
    public Vector4 Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x14)]
public struct D2Class_86268080
{
    [SchemaField(0x4)]
    public Tag Unk04;
}

[StructLayout(LayoutKind.Sequential, Size = 0x210)]
public struct D2Class_72268080
{
    [SchemaField(0x200)]
    public TigerHash Unk200;
    public int Unk204;
    public int Unk208;
}

[StructLayout(LayoutKind.Sequential, Size = 0x348)]
public struct D2Class_3C268080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0xC30)]
public struct D2Class_B1288080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0xA8)]
    public DynamicArray<D2Class_0F008080> UnkA8;
    [SchemaField(0xB8)]
    public DynamicArray<D2Class_BC288080> UnkB8;
    [SchemaField(0xC8)]
    public DynamicArray<D2Class_BE288080> UnkC8;
    [SchemaField(0x790)]
    public DynamicArray<D2Class_E4288080> Unk790;
    [SchemaField(0x7D0)]
    public DynamicArray<D2Class_E4288080> Unk7D0;
    [SchemaField(0x810)]
    public DynamicArray<D2Class_E4288080> Unk810;
}

[StructLayout(LayoutKind.Sequential, Size = 0xC)]
public struct D2Class_BC288080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_BE288080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x38)]
public struct D2Class_E4288080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x4C0)]
public struct D2Class_9B288080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x270)]
    public DynamicArray<D2Class_2B948080> Unk270;
}

[StructLayout(LayoutKind.Sequential, Size = 0x100)]
public struct D2Class_2B948080
{
    // Some set of vectors with rotation and translation data but interspersed with other data
}

[StructLayout(LayoutKind.Sequential, Size = 0x160)]
public struct D2Class_32288080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x108)]
public struct D2Class_31288080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0xE0)]
public struct D2Class_9C818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x108)]
public struct D2Class_9D818080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x38)]
    public DynamicArray<D2Class_F79A8080> Unk38;
    [SchemaField(0xB8)]
    public DynamicArray<D2Class_A9818080> UnkB8;
}

[StructLayout(LayoutKind.Sequential, Size = 0x30)]
public struct D2Class_A9818080
{
    public Vector4 Unk00;
    public Vector4 Unk10;
    public int Unk20;
    public int Unk24;
    public int Unk28;
    public int Unk2C;
}

[StructLayout(LayoutKind.Sequential, Size = 0x940)]
public struct D2Class_DA5E8080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x240)]
public struct D2Class_DB5E8080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;
}

    [StructLayout(LayoutKind.Sequential, Size = 0x50)]
public struct D2Class_12848080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x30)]
    public DynamicArray<D2Class_1A848080> Unk30;
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_1A848080
{
    public TigerHash Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0xA0)]
public struct D2Class_0E848080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x38)]
    public DynamicArray<D2Class_F79A8080> Unk38;
    [SchemaField(0x88)]
    public DynamicArray<D2Class_1B848080> Unk88;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_1B848080
{
    public int Unk00;
    public float Unk04;
    public DynamicArray<D2Class_1D848080> Unk08;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_1D848080
{
    public int Unk00;
    public int Unk04;
    public Tag Unk08;
    public int Unk0C;
    public int Unk10;
    public int Unk14;
}

[StructLayout(LayoutKind.Sequential, Size = 0x40)]
public struct D2Class_21868080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x2C0)]
public struct D2Class_6A918080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x1D0)]
    public DynamicArray<D2Class_07008080> Unk1D0;
    [SchemaField(0x1F0)]
    public DynamicArray<D2Class_07008080> Unk1F0;
}

[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct D2Class_07008080
{
    public uint Unk00;
}

[StructLayout(LayoutKind.Sequential, Size = 0x4D8)]
public struct D2Class_46868080
{
    [DestinyField(FieldType.ResourceInTag)]
    public dynamic? Unk00;

    [SchemaField(0x38)]
    public DynamicArray<D2Class_F79A8080> Unk38;
    [SchemaField(0x480)]
    public DynamicArray<D2Class_77878080> Unk480;
    [SchemaField(0x4C0)]
    public DynamicArray<D2Class_37878080> Unk4C0;
}

[StructLayout(LayoutKind.Sequential, Size = 0x90)]
public struct D2Class_77878080
{
    public TigerHash Unk00;
    public TigerHash Unk04;
    [SchemaField(0x14)]
    public float Unk14;
    public long Unk18;
    public Vector4 Unk20;
    public Vector4 Unk30;
    public Vector4 Unk40;
    public Vector4 Unk50;
    public Vector4 Unk60;
    public Vector4 Unk70;
    public int Unk80;
    public int Unk84;
    public int Unk88;
    public int Unk8C;
}


[StructLayout(LayoutKind.Sequential, Size = 4)]
public struct D2Class_37878080
{
    public Tag Unk00;
}

// [StructLayout(LayoutKind.Sequential, Size = 0x800)]
// public struct D2Class_20408080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
//
//     [SchemaField(0x350)]
//     public DynamicArray<D2Class_DE408080> Unk350;
//     [SchemaField(0x380)]
//     public DynamicArray<D2Class_28218080> Unk380;
//     [SchemaField(0x3D0)]
//     public DynamicArray<D2Class_E0408080> Unk3D0;
//     [SchemaField(0x7A0)]
//     public DynamicArray<D2Class_51408080> Unk7A0;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 4)]
// public struct D2Class_DE408080
// {
//     public float Unk00;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x28)]
// public struct D2Class_28218080
// {
//     public float Unk00;
//     public float Unk04;
//     public float Unk08;
//     // [D2FieldOffset(0x10), D2FieldType(26)]
//     // public byte Unk0x10;
//     [SchemaField(0x20)]
//     public uint Unk20;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x70)]
// public struct D2Class_E0408080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x40)]
// public struct D2Class_51408080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
// }

// [StructLayout(LayoutKind.Sequential, Size = 0x668)]
// public struct D2Class_FC3F8080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
//
//     [SchemaField(0x1D0)]
//     public DynamicArray<D2Class_91408080> Unk1D0;
//     [SchemaField(0x1F0)]
//     public DynamicArray<D2Class_3B408080> Unk1F0;
//     [SchemaField(0x390)]
//     public DynamicArray<D2Class_1C408080> Unk390;
//     [SchemaField(0x3D0)]
//     public DynamicArray<D2Class_E1408080> Unk3D0;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x160)]
// public struct D2Class_91408080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
//
//     [SchemaField(0x78)]
//     public DynamicArray<D2Class_74408080> Unk78;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x10)]  // size is actually 8 but we need this to make it work
// public struct D2Class_74408080
// {
//     [SchemaField(0xC), DestinyField(FieldType.Resource)]
//     public dynamic? UnkC;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x38)]
// public struct D2Class_8D408080
// {
//     [SchemaField(0x10)]
//     public float Unk10;
//     [SchemaField(0x20), DestinyField(FieldType.FileHash)]
//     public Tag Unk20;
//     [SchemaField(0x30)]
//     public TigerHash Unk30;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0xC)]  // Size is actually 8 but we need this to make it work
// public struct D2Class_3B408080
// {
//     [SchemaField(0x8), DestinyField(FieldType.Resource)]
//     public dynamic? UnkC;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x10)]
// public struct D2Class_42408080
// {
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0xC)]
// public struct D2Class_1C408080
// {
//     public TigerHash Unk00;
//     public int Unk04;
//     public int Unk08;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x78)]
// public struct D2Class_E1408080
// {
//     [DestinyField(FieldType.ResourceInTag)]
//     public dynamic? Unk00;
//
//     // loads of floats
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x28)]
// public struct D2Class_FD3F8080
// {
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0xB00)]
// public struct D2Class_C5348080
// {
//     public D2Class_052E8080 Unk0x0;
//     public byte UnkAF4;
//     public byte UnkAF8;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0xB00)]
// public struct D2Class_052E8080
// {
//     [SchemaField(0x650)]
//     public byte Unk650;
//     [SchemaField(0x750)]
//     public byte Unk750;
//     [SchemaField(0x751)]
//     public bool Unk751;
//     [SchemaField(0x7D2)]
//     public bool Unk7D2;
//     [SchemaField(0x800)]
//     public float Unk800;
//     // [D2FieldOffset(0x804), D2FieldType(44)]
//     // public byte Unk0x804;
//     // [D2FieldOffset(0x808), D2FieldType(45)]
//     // public byte Unk0x808;
//     [SchemaField(0x768)]
//     public DynamicArray<D2Class_D2398080> Unk768;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x30)]
// public struct D2Class_D2398080
// {
//     // [D2FieldOffset(0x20), D2FieldType(32)]  // resource pointer
//     // public byte Unk0x20;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x70)]
// public struct D2Class_64338080
// {
//     [SchemaField(0x48)]
//     public DynamicArray<D2Class_71338080> Unk48;
// }
//
// [StructLayout(LayoutKind.Sequential, Size = 0x30)]
// public struct D2Class_71338080
// {
// }

// General, parents that reference Entity

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public struct D2Class_30898080
{
    public long FileSize;
    public DynamicArray<D2Class_34898080> Unk08;
    public DynamicArray<D2Class_33898080> Unk18;
}

[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_34898080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_33898080
{
    public StringPointer TagPath;
    [Tag64]
    public Tag Tag;  // if .pattern.tft, then Entity - if .budget_set.tft, then parent of itself
    public StringPointer TagNote;
}

[StructLayout(LayoutKind.Sequential, Size = 0x58)]
public struct D2Class_ED9E8080
{
    public long FileSize;
    [SchemaField(0x18)]
    public Tag Unk18;
    [SchemaField(0x28)]
    public DynamicArray<D2Class_F19E8080> Unk28;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_F19E8080
{
    [DestinyField(FieldType.RelativePointer)]
    public string TagPath;
    [Tag64]
    public Tag Tag;  // if .pattern.tft, then Entity
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_7E988080
{
    public Tag Unk00;
    public Tag Unk08;
}

public struct VertexWeight
{
    public IntVector4 WeightValues;
    public IntVector4 WeightIndices;
}

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct D2Class_44318080
{
    public long FileSize;
    [Tag64]
    public Entity? Entity;
}

#region Named entities

//I think this is the old struct for named bags, it seems like it changed to 1D478080?

//[StructLayout(LayoutKind.Sequential, Size = 0x50)]
//public struct D2Class_75988080
//{
//    public long FileSize;
//    // [DestinyField(FieldType.RelativePointer)]
//    // public string DestinationGlobalTagBagName;
//    public FileHash DestinationGlobalTagBag;
//    // [SchemaField(0x20)]
//    // public FileHash PatrolTable1;
//    // [SchemaField(0x28), DestinyField(FieldType.RelativePointer)]
//    // public string PatrolTableName;
//    // public FileHash PatrolTable2;
//}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_1D478080
{
    public long FileSize;
    public DynamicArray<D2Class_D3598080> DestinationGlobalTagBags;
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_D3598080
{
    public FileHash DestinationGlobalTagBag;
    [SchemaField(0x8), DestinyField(FieldType.RelativePointer)]
    public string DestinationGlobalTagBagName;
}

#endregion

#region Audio

[StructLayout(LayoutKind.Sequential, Size = 0x6b8)]
public struct D2Class_6E358080
{
    [SchemaField(0x648)]
    public DynamicArray<D2Class_9B318080> PatternAudioGroups;
}

[StructLayout(LayoutKind.Sequential, Size = 0x128)]
public struct D2Class_9B318080
{
    public TigerHash WeaponContentGroup1Hash;
    [SchemaField(0x8)]
    public TigerHash Unk08;
    [SchemaField(0x18), Tag64]
    public FileHash StringContainer;  // idk why but i presume debug strings
    public TigerHash WeaponContentGroup2Hash;  // "weaponContentGroupHash" from API
    // theres other stringcontainer stuff but skipping it
    [SchemaField(0xA0), Tag64]
    public Entity? WeaponSkeletonEntity;
    [SchemaField(0xD0), Tag64]
    public Tag<D2Class_A36F8080> AudioGroup;
    public float UnkE0;
    [SchemaField(0x110)]
    public float Unk110;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_0D8C8080
{
    public long FileSize;
    public DynamicArray<D2Class_0F8C8080> Audio;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_0F8C8080
{
    public TigerHash WwiseEventHash;
    [SchemaField(0x8)]
    public DynamicArray<D2Class_138C8080> Sounds;
}

[StructLayout(LayoutKind.Sequential, Size = 0x28)]
public struct D2Class_138C8080
{
    public short Unk00;
    public short Unk02;
    [SchemaField(0x8)]
    public TigerHash Unk08;
    [SchemaField(0x10)]
    public StringPointer WwiseEventName;
    [Tag64]
    public WwiseSound Sound;
}

[StructLayout(LayoutKind.Sequential, Size = 0x540)]
public struct D2Class_97318080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0xB0)]
public struct D2Class_F62C8080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x338)]
public struct D2Class_F42C8080
{
    [SchemaField(0x2c8)]
    public DynamicArray<D2Class_FA2C8080> PatternAudioGroups;
}

[StructLayout(LayoutKind.Sequential, Size = 0x258)]
public struct D2Class_FA2C8080
{
    [SchemaField(0x10)]
    public TigerHash WeaponContentGroupHash; // "weaponContentGroupHash" from API
    public TigerHash Unk14;
    public TigerHash Unk18;
    [SchemaField(0x30)]
    public TigerHash WeaponTypeHash1; // "weaponTypeHash" from API
    public byte Unk34;
    public byte Unk35;
    public byte Unk36;
    public byte Unk37;
    public float Unk38;
    public float Unk3C;
    public float Unk40;
    [SchemaField(0x48)]
    public int Unk48;
    [SchemaField(0x50)]
    public TigerHash Unk50;
    [SchemaField(0x60), Tag64]
    public Tag Unk60;
    [SchemaField(0x78), Tag64]
    public Tag Unk78;
    [SchemaField(0x90), Tag64]
    public Tag Unk90;
    [SchemaField(0xA8), Tag64]
    public Tag UnkA8;
    [SchemaField(0xC0), Tag64]
    public Tag UnkC0;
    [SchemaField(0xD8), Tag64]
    public Tag UnkD8;
    [SchemaField(0xF0), Tag64]
    public Tag<D2Class_A36F8080> AudioEntityParent;
    [SchemaField(0x120)]
    public TigerHash WeaponTypeHash2; // "weaponTypeHash" from API
    [SchemaField(0x130), Tag64]
    public Tag Unk130;
    [SchemaField(0x148), Tag64]
    public Tag Unk148;
    [SchemaField(0x168)]
    public int Unk168;
    [SchemaField(0x178)]
    public int Unk178;
    [SchemaField(0x180)]
    public float Unk180;
    public float Unk184;
    public float Unk188;
    public int Unk18C;
    public int Unk190;
    public float Unk194;
    public float Unk198;
    [SchemaField(0x1A8)]
    public int Unk1A8;
    public float Unk1AC;
    [SchemaField(0x1C0), Tag64]
    public Tag Unk1C0;
    [SchemaField(0x1D8), Tag64]
    public Tag Unk1D8;

    // public DynamicArray<D2Class_87978080> Unk1E8;
    // public DynamicArray<D2Class_84978080> Unk1F8;
    // public DynamicArray<D2Class_062D8080> Unk208;
    [SchemaField(0x248), Tag64]
    public Tag Unk248;
}


[StructLayout(LayoutKind.Sequential, Size = 0xA0)]
public struct D2Class_2D098080
{
    public long FileSize;
    public TigerHash Unk08;
    [SchemaField(0x18), Tag64]
    public Entity? Unk18;
    [SchemaField(0x30), Tag64]
    public Entity? Unk30;
    [SchemaField(0x48), Tag64]
    public Entity? Unk48;
    [SchemaField(0x60), Tag64]
    public Entity? Unk60;
    [SchemaField(0x78), Tag64]
    public Entity? Unk78;
    [SchemaField(0x90), Tag64]
    public Entity? Unk90;
}

[StructLayout(LayoutKind.Sequential, Size = 0x390)]
public struct D2Class_79818080
{
    [SchemaField(0x1a8)]
    public DynamicArray<D2Class_F1918080> WwiseSounds1;
    public DynamicArray<D2Class_F1918080> WwiseSounds2;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_F1918080
{
    [SchemaField(0x10)]
    public ResourcePointer Unk10;
}


[StructLayout(LayoutKind.Sequential, Size = 0x68)]
public struct D2Class_40668080
{
    [SchemaField(0x28), Tag64]
    public WwiseSound Sound;
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_72818080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x20)]
public struct D2Class_00488080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x300)]
public struct D2Class_79948080
{
}

[StructLayout(LayoutKind.Sequential, Size = 0x40)]
public struct D2Class_E3918080
{
}

#endregion