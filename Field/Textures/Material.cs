﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Field.General;
using Field.Textures;
using Field.Utils;
using File = System.IO.File;

namespace Field;

public struct ExportSettings { public bool Raw, Unreal, Blender, Source2; }

public class Material : Tag {
    
    public D2Class_AA6D8080 Header;
    
    private static readonly object Lock = new();

    public Material(TagHash hash) : base(hash) { }

    protected override void ParseStructs() {
        Header = ReadHeader<D2Class_AA6D8080>();
    }

    public void Export(string savePath, ExportSettings settings) {
        var texturePath = $"{savePath}/Textures";
        Directory.CreateDirectory(texturePath);
        var matPath = $"{savePath}/Materials";
        Directory.CreateDirectory(matPath);
        SaveAllTextures(texturePath, Header.PSTextures, Header.VSTextures, Header.CSTextures);
        if(settings.Raw) {
            var rawPath = $"{matPath}/Raw";
            Directory.CreateDirectory(rawPath);
            ExportMaterialRaw(rawPath);
            Console.WriteLine($"Successfully exported raw Material {Hash}.");
        }
        if(settings.Blender) {
            var blenderPath = $"{matPath}/Blender";
            Directory.CreateDirectory(blenderPath);
            ExportMaterialBlender(blenderPath);
            Console.WriteLine($"Successfully exported Blender Material {Hash}.");
        }
        if(settings.Unreal) {
            var unrealPath = $"{matPath}/Unreal";
            Directory.CreateDirectory(unrealPath);
            ExportMaterialUnreal(unrealPath);
            Console.WriteLine($"Successfully exported Unreal Material {Hash}.");
        }
        if(settings.Source2) {
            var s2Path = $"{matPath}/Source2";
            Directory.CreateDirectory(s2Path);
            ExportMaterialSource2(s2Path);
            Console.WriteLine($"Successfully exported Source2 Material {Hash}.");
        }
    }

    private byte[] GetBytecode(ShaderType type) {
        var path = GetTempPath(type);
        lock (Lock) {
            if (!File.Exists(path)) {
                var bytecode = type switch {
                    ShaderType.Pixel => Header.PixelShader.GetBytecode(),
                    ShaderType.Vertex => Header.VertexShader.GetBytecode(),
                    _ => Header.ComputeShader.GetBytecode()
                };
                Directory.GetParent(path)?.Create();
                File.WriteAllBytes(path, bytecode);
                return bytecode;
            }
            return File.ReadAllBytes(path);
        }
    }
    
    private string Disassemble(ShaderType type) {
        var fileName = GetTempPath(type, ".asm");
        if (!File.Exists(fileName)) {
            var data = DirectX.DisassembleDXBC(GetBytecode(type));
            File.WriteAllText(fileName, data);
            return data;
        }
        return File.ReadAllText(fileName);
    }
    
    private string Decompile(ShaderType type, bool allowretry = true) {
        var binFile = GetTempPath(type);
        var hlslFile = GetTempPath(type, ".hlsl");

        if (!File.Exists(hlslFile)) {
            var startInfo = new ProcessStartInfo {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "3dmigoto_shader_decomp.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = $"-D {binFile}"
            };

            using (var exeProcess = Process.Start(startInfo)) {
                exeProcess?.WaitForExit();
            }

            if (!File.Exists(hlslFile)) {
                if (allowretry && File.Exists(binFile)) {
                    File.Delete(binFile);
                    GetBytecode(type);
                    return Decompile(type, false);
                }
                throw new FileNotFoundException($"Decompilation failed for {Hash}");
            }
        }
        
        lock (Lock) {
            var hlsl = "";
            while (hlsl == "") {
                try { hlsl = File.ReadAllText(hlslFile); }
                catch (IOException) { Thread.Sleep(100); }
            }
            return hlsl;
        }
    }
    
    private static void SaveAllTextures(string saveDirectory, params List<D2Class_CF6D8080>[] shaderTextures) {
        foreach(var textures in shaderTextures) {
            foreach (var e in textures.Where(e => e.Texture != null)) {
                var path = $"{saveDirectory}/{e.Texture.Hash}";
                if (!File.Exists(path + GetTextureExtension()))
                    e.Texture.SavetoFile(path);
            }
        }
    }

    #region Platform-specific exports

    private void ExportMaterialRaw(string path) {
        if(Header.PixelShader != null) {
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.hlsl", Decompile(ShaderType.Pixel)); }
            catch (IOException) { }
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.asm", Disassemble(ShaderType.Pixel)); }
            catch (IOException) { }
            Console.WriteLine($"Exported raw PixelShader for Material {Hash}.");
        }
        if(Header.VertexShader != null) {
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Vertex)}_{Hash}.hlsl", Decompile(ShaderType.Vertex)); }
            catch (IOException) { }
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Vertex)}_{Hash}.asm", Disassemble(ShaderType.Vertex)); }
            catch (IOException) { }
            Console.WriteLine($"Exported raw VertexShader for Material {Hash}.");
        }
        if(Header.ComputeShader != null) {
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Compute)}_{Hash}.hlsl", Decompile(ShaderType.Compute)); }
            catch (IOException) { }
            try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Compute)}_{Hash}.asm", Disassemble(ShaderType.Compute)); }
            catch (IOException) { }
            Console.WriteLine($"Exported raw ComputeShader for Material {Hash}.");
        }
    }

    private void ExportMaterialBlender(string path) {
        // TODO: Merge Vertex and Pixel Shaders if applicable
        // TODO: Create a proper import script, assuming textures are bound and loaded
        if(Header.PixelShader != null) {
            string bpy = new ASM_Parser(Disassemble(ShaderType.Vertex), this, false).ToNodeScript();
            if (bpy != string.Empty) {
                try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.py", bpy); }
                catch (IOException) { }
                Console.WriteLine($"Exported Blender PixelShader {Hash}.");
            }
        }
        if(Header.VertexShader != null) {
            //(this, $"{path}/../..", Disassemble(ShaderType.Vertex), true);

            string bpy = new ASM_Parser(Disassemble(ShaderType.Vertex), this, true).ToNodeScript();
            if(bpy != string.Empty) {
                try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Vertex)}_{Hash}.py", bpy); }
                catch (IOException) { }
                Console.WriteLine($"Exported Blender VertexShader {Hash}.");
            }
        }
        // I don't think Blender can even handle compute shaders, so I'll leave that out. If it can, it's the same as above.
    }

    private void ExportMaterialUnreal(string path) {
        if(Header.PixelShader != null) {
            var usf = new UsfConverter().HlslToUsf(this, Decompile(ShaderType.Pixel), false);
            if(usf != string.Empty) {
                try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.usf", usf); }
                catch (IOException) { }
                Console.WriteLine($"Exported Unreal PixelShader {Hash}.");
            }
        }
        if(Header.VertexShader != null) {
            var usf = new UsfConverter().HlslToUsf(this, Decompile(ShaderType.Vertex), true);
            if(usf != string.Empty) {
                try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Vertex)}_{Hash}.usf", usf); }
                catch (IOException) { }
                Console.WriteLine($"Exported Unreal VertexShader {Hash}.");
            }
        }
    }

    private void ExportMaterialSource2(string path) {
        if(Header.PixelShader != null) {
            var vfx = new VfxConverter().HlslToVfx(this, Decompile(ShaderType.Pixel), false);
            if(vfx != string.Empty) {
                try { File.WriteAllText($"{path}/{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.vfx", vfx); }
                catch (IOException) { }
                Console.WriteLine($"Exported Source 2 PixelShader {Hash}.");
            }
            var materialBuilder = new StringBuilder("Layer0 \n{");
            materialBuilder.AppendLine($"\n\tshader \"{GetShaderPrefix(ShaderType.Pixel)}_{Hash}.vfx\"");
            materialBuilder.AppendLine("\tF_ALPHA_TEST 1");
            foreach(var e in Header.PSTextures.Where(e => e.Texture != null))
                materialBuilder.AppendLine($"\tTextureT{e.TextureIndex} \"materials/Textures/{e.Texture.Hash}{GetTextureExtension()}\"");
            materialBuilder.AppendLine("}");
            Directory.CreateDirectory($"{path}/materials");
            try { File.WriteAllText($"{path}/materials/{Hash}.vmat", materialBuilder.ToString()); }
            catch (IOException) { }
        }
    }
    
    #endregion

    #region Utils
    
    public static string GetTextureExtension() {
        return TextureExtractor.Format switch {
            ETextureFormat.PNG => ".png",
            ETextureFormat.TGA => ".tga",
            _ => ".dds"
        };
    }

    private static string GetShaderPrefix(ShaderType type) {
        return type switch {
            ShaderType.Pixel => "PS",
            ShaderType.Vertex => "VS",
            _ => "PS"
        };
    }
    
    private string GetTempPath(ShaderType type, string extension = ".bin") {
        var dir = $"{Path.GetTempPath()}/CharmCache/Shaders";
        var path = $"{dir}/{GetShaderPrefix(type)}_{Hash}{extension}";
        if(!File.Exists(dir))
            Directory.CreateDirectory(dir);
        return path;
    }

    public enum ShaderType { Pixel, Vertex, Compute }
    
    #endregion

    public List<D2Class_CF6D8080> GetTextures(ShaderType type) {
        return type switch {
            Material.ShaderType.Pixel => Header.PSTextures,
            Material.ShaderType.Vertex => Header.VSTextures,
            _ => Header.CSTextures
        };
    }

    public bool HasShaderType(ShaderType type) {
        return type switch {
            Material.ShaderType.Pixel => Header.PixelShader != null,
            Material.ShaderType.Vertex => Header.VertexShader != null,
            _ => Header.ComputeShader != null
        };
    }
}

#region Symmetry

[StructLayout(LayoutKind.Sequential, Size = 0x3D0)]
public struct D2Class_AA6D8080
{
    public long FileSize;
    public uint Unk08;
    public uint Unk0C;
    public uint Unk10;

    [DestinyOffset(0x70), DestinyField(FieldType.TagHash)]
    public ShaderHeader VertexShader;
    [DestinyOffset(0x78), DestinyField(FieldType.TablePointer)]
    public List<D2Class_CF6D8080> VSTextures;
    [DestinyOffset(0x90), DestinyField(FieldType.TablePointer)]
    public List<D2Class_09008080> Unk90;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> UnkA0;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_3F018080> UnkB0;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> UnkC0;
    
    [DestinyOffset(0x2B0), DestinyField(FieldType.TagHash)]
    public ShaderHeader PixelShader;
    [DestinyOffset(0x2B8), DestinyField(FieldType.TablePointer)]
    public List<D2Class_CF6D8080> PSTextures;
    [DestinyOffset(0x2D0), DestinyField(FieldType.TablePointer)]
    public List<D2Class_09008080> Unk2D0;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> Unk2E0;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_3F018080> Unk2F0;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> Unk300;
    [DestinyOffset(0x324)] 
    public TagHash PSVector4Container;
    
    [DestinyOffset(0x340), DestinyField(FieldType.TagHash)]
    public ShaderHeader ComputeShader;
    [DestinyOffset(0x348), DestinyField(FieldType.TablePointer)]
    public List<D2Class_CF6D8080> CSTextures;
    [DestinyOffset(0x360), DestinyField(FieldType.TablePointer)]
    public List<D2Class_09008080> Unk360;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> Unk370;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_3F018080> Unk380;
    [DestinyField(FieldType.TablePointer)]
    public List<D2Class_90008080> Unk390;
    
}

[StructLayout(LayoutKind.Sequential, Size = 0x18)]
public struct D2Class_CF6D8080
{
    public long TextureIndex;
    [DestinyField(FieldType.TagHash64)]
    public TextureHeader Texture;
}

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct D2Class_09008080
{
    public byte Value;
}

[StructLayout(LayoutKind.Sequential, Size = 0x10)]
public struct D2Class_3F018080
{
    [DestinyField(FieldType.TagHash64)]
    public Tag Unk00;
}

#endregion
