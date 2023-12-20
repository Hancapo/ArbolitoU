﻿using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SharpDX;
using static System.Media.SystemSounds;
using Color = Avalonia.Media.Color;

namespace ArbolitoU.Pages;

public partial class Settings : UserControl
{
    private static Window? _mainWindow = ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!)?.MainWindow;
    public Settings()
    {
        InitializeComponent();
        try
        {
            tbGTApath.Text = Program.ArbolitoSettings?._ArbolitoSettings.gtapath;
            tbOutputPath.Text = Program.ArbolitoSettings?._ArbolitoSettings.outputpath;
        }
        catch (Exception e)
        {
            // ignored
        }
    }
    private async void BtnGTAsearch_OnClick(object? sender, RoutedEventArgs e)
    {
        var folder = await _mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select your GTAV Path"
        });
        
        if (!folder.Any()) return;
        var gtaPath = folder[0].Path.LocalPath;
        Program.ArbolitoSettings._ArbolitoSettings.gtapath = gtaPath;
        if (!ValidateGtaPath(gtaPath) && !string.IsNullOrEmpty(gtaPath))
        {
            var invalidPathDialog = new ContentDialog()
            {
                Title = "Invalid GTA Path",
                Content = "The GTA Path is invalid",
                PrimaryButtonText = "Ok"
            };
            Exclamation.Play();
            await invalidPathDialog.ShowAsync();
        }
    }

    private async void BtnOutputSearch_OnClick(object? sender, RoutedEventArgs e)
    {
        var folder = await _mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Select your Output Path"
        });
        
        if (!folder.Any()) return;
        var outputPath = folder[0].Path.LocalPath;
        
        if(Directory.Exists(outputPath) && !string.IsNullOrEmpty(outputPath))
        {
            tbOutputPath.Text = outputPath;
            if (Program.ArbolitoSettings != null) Program.ArbolitoSettings._ArbolitoSettings.outputpath = outputPath;
        }
        
        
    }

    private static bool ValidateGtaPath(string? gtaPath)
    {
        return gtaPath != null && File.Exists(Path.Combine(gtaPath, "GTA5.exe"));
    }

    private async void BtnSaveSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ValidateGtaPath(Program.ArbolitoSettings?._ArbolitoSettings.gtapath) && !string.IsNullOrEmpty(Program.ArbolitoSettings?._ArbolitoSettings.outputpath) && Directory.Exists(tbOutputPath.Text))
        {
            var confirmSaveDialog = new ContentDialog()
            {
                Title = "Save Settings",
                Content = "Are you sure you want to save the settings?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No"
            };

            Hand.Play();
            var result = await confirmSaveDialog.ShowAsync();

            if (result != ContentDialogResult.Primary) return;
            Program.SaveJsonSettings();

            var savedDialog = new ContentDialog()
            {
                Title = "Settings Saved",
                Content = "The settings have been saved",
                PrimaryButtonText = "Ok"
            };
            Asterisk.Play();
            await savedDialog.ShowAsync();
        }
        else
        {
            var invalidSaveDialog = new ContentDialog()
            {
                Title = "Invalid Settings",
                Content = "The settings are invalid or incomplete",
                PrimaryButtonText = "Ok"
            };
            Exclamation.Play();
            await invalidSaveDialog.ShowAsync();
        }
    }

    private async void BtnResetSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var resetSettingsDialog = new ContentDialog()
        {
            Title = "Reset Settings",
            Content = "Are you sure you want to reset the settings? This will close the program",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No",
        };
        
        
        var result = await resetSettingsDialog.ShowAsync();
        
        if (result == ContentDialogResult.Primary)
        {
            tbGTApath.Text = "";
            tbOutputPath.Text = "";
            Program.ResetJsonSettings();
            Asterisk.Play();
            _mainWindow.Close();
        }
        
    }
}