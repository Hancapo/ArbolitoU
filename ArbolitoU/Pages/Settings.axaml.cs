using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using static System.Media.SystemSounds;

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
        if (ValidateGtaPath(gtaPath) && !string.IsNullOrEmpty(gtaPath))
        {
            var validPathMsg = MessageBoxManager.GetMessageBoxStandard("Valid GTA Path", "The GTA Path is valid", ButtonEnum.Ok, Icon.Info);
            await validPathMsg.ShowAsync();
            tbGTApath.Text = gtaPath;
        }
        else
        {
            var invalidPathMsg = MessageBoxManager.GetMessageBoxStandard("Invalid GTA Path", "The GTA Path is invalid", ButtonEnum.Ok, Icon.Error);
            await invalidPathMsg.ShowAsync();
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
            var validOutpatPathMsg = MessageBoxManager.GetMessageBoxStandard("Valid Output Path", "The Output Path is valid", ButtonEnum.Ok, Icon.Info);
            await validOutpatPathMsg.ShowAsPopupAsync(this);
            tbOutputPath.Text = outputPath;
            Program.ArbolitoSettings._ArbolitoSettings.outputpath = outputPath;

        }
        else
        {
            var invalidOutpatPathMsg = MessageBoxManager.GetMessageBoxStandard("Invalid Output Path", "The Output Path is invalid", ButtonEnum.Ok, Icon.Error);
            await invalidOutpatPathMsg.ShowAsPopupAsync(this);
        }
        
        
    }

    private static bool ValidateGtaPath(string? gtaPath)
    {
        return gtaPath != null && File.Exists(Path.Combine(gtaPath, "GTA5.exe"));
    }

    private void BtnSaveSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        if (ValidateGtaPath(Program.ArbolitoSettings._ArbolitoSettings.gtapath) && !string.IsNullOrEmpty(Program.ArbolitoSettings._ArbolitoSettings.outputpath) && Directory.Exists(tbOutputPath.Text))
        {
            var confirmSaveMsg = MessageBoxManager.GetMessageBoxStandard("Save Settings", "Are you sure you want to save the settings?", ButtonEnum.YesNo, Icon.Info);
            Hand.Play();
            confirmSaveMsg.ShowAsPopupAsync(_mainWindow).ContinueWith(task =>
            {
                if (task.Result == ButtonResult.Yes)
                {
                    Program.SaveJsonSettings();
                    var savedMsg = MessageBoxManager.GetMessageBoxStandard("Settings Saved", "The settings have been saved", ButtonEnum.Ok, Icon.Info);
                    savedMsg.ShowAsPopupAsync(_mainWindow);
                    Asterisk.Play();
                }
            });
        }
    }

    private void BtnResetSettings_OnClick(object? sender, RoutedEventArgs e)
    {
        var confirmResetMsg = MessageBoxManager.GetMessageBoxStandard("Reset Settings", "Are you sure you want to reset the settings?", ButtonEnum.YesNo, Icon.Info);
        confirmResetMsg.ShowAsPopupAsync(_mainWindow).ContinueWith(task =>
        {
            Hand.Play();
            if (task.Result == ButtonResult.Yes)
            {
                Program.ResetJsonSettings();
                tbGTApath.Text = "";
                tbOutputPath.Text = "";
                var resetMsg = MessageBoxManager.GetMessageBoxStandard("Settings Reset", "The settings have been reset", ButtonEnum.Ok, Icon.Info);
                resetMsg.ShowAsPopupAsync(_mainWindow);
                Asterisk.Play();
            }
        });
    }
}