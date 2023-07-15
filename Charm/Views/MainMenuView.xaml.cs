﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Tiger.Schema;
using Tiger;

namespace Charm.Views;


public partial class MainMenuView : UserControl
{
    public string Hash { get; set; } = "2E24C680";

    public MainMenuView()
    {
        InitializeComponent();
    }

    private void SaveDatabase_OnClick(object? sender, RoutedEventArgs e)
    {
        string fileHash = "17A7B580";
        StaticMapData mapData = FileResourcer.Get().GetFile<StaticMapData>(fileHash);
        FbxHandler handler = new();
        // mapData.LoadArrangedIntoFbxScene(handler);
        mapData.LoadIntoFbxScene(handler, "TestModels/TestMap", true);
        handler.ExportScene("TestModels/TestMap.fbx");
        throw new Exception();
        // Strategy.SetStrategy(TigerStrategy.DESTINY2_WITCHQUEEN_6307);
        // var x = PackageResourcer.Get();
        // byte[] data = PackageResourcer.Get().GetFileData(new FileHash(Hash));
        // string tempFilePath = $"./TempFiles/{Hash}.bin";
        // Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
        // File.WriteAllBytes(tempFilePath, data);
        // new Process
        // {
        //     StartInfo = new ProcessStartInfo($@"{Path.GetFullPath(tempFilePath)}")
        //     {
        //         UseShellExecute = true
        //     }
        // }.Start();
    }

    private void OpenHash_OnClick(object? sender, RoutedEventArgs e)
    {
        var x = PackageResourcer.Get();
        byte[] data = PackageResourcer.Get().GetFileData(new FileHash(Hash));
        string tempFilePath = $"./TempFiles/{Hash}.bin";
        Directory.CreateDirectory(Path.GetDirectoryName(tempFilePath));
        File.WriteAllBytes(tempFilePath, data);
        new Process
        {
            StartInfo = new ProcessStartInfo($@"{Path.GetFullPath(tempFilePath)}")
            {
                UseShellExecute = true
            }
        }.Start();
    }
}