using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbolitoU.Pages;
using ArbolitoU.Windows;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Windowing;
using Microsoft.Win32;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;

namespace ArbolitoU;

public partial class MainWindow : AppWindow
{
    List<string> _ymapFiles = new();
    List<string> _ytypFiles = new();
    List<string> _trainTracksFiles = new();
    string _textFile = "";

    public MainWindow()
    {
        InitializeComponent();
        if (!File.Exists("appsettings.json"))
        {
            Program.ArbolitoSettings = new ArbolitoSettings()
            {
                _ArbolitoSettings = new SettingsContainer(){gtapath = "", outputpath = ""}
            };
            Program.SaveJsonSettings();
        }
        SplashScreen = new ArbolitoSplashScreen(this);
        HomeItem.IsSelected = true;
        ArbolitoFrame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
    }


    private void HomeItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (ArbolitoFrame.Content?.GetType() != typeof(Home))
            ArbolitoFrame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
    }

    private void FileExtItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (ArbolitoFrame.Content?.GetType() != typeof(FileExtractor))
            ArbolitoFrame.Navigate(typeof(FileExtractor), null, new DrillInNavigationTransitionInfo());
    }

    private void MenuItemQuit_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Close();
    }

    private async void MiSelectYMAP_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var ymapSelection = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select YMAP(s) file(s)",
            AllowMultiple = true,
            FileTypeFilter = new[] { new FilePickerFileType("YMAP(s)") { Patterns = new[] { "*.ymap" } } }
        });
    }

    private void MiSelectYTYP_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var ytypSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select YTYP(s) file(s)",
            AllowMultiple = true,
            FileTypeFilter = new[] { new FilePickerFileType("YTYP(s)") { Patterns = new[] { "*.ytyp" } } }
        });
    }

    private void MiSelectTrainTracks_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var trainTracksSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select Train Tracks file(s)",
            AllowMultiple = true,
            FileTypeFilter = new[] { new FilePickerFileType("Train Tracks") { Patterns = new[] { "*.dat" } } }
        });
    }

    private void MiSelectTextFile_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var textFileSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select Text file",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("Text File") { Patterns = new[] { "*.txt" } } }
        });
    }

    private void SettingsItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (ArbolitoFrame.Content?.GetType() != typeof(Settings))
            ArbolitoFrame.Navigate(typeof(Settings), null, new DrillInNavigationTransitionInfo());
    }

    private void MenuItemWiam_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var wiamWindow = new WiamWindow
        {
            Height = 500,
            Width = 400,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            Title = "WIAM 🐈",
            CanResize = false
        };
        wiamWindow.ShowDialog(this);
    }
}

internal class ArbolitoSplashScreen(MainWindow owner) : IApplicationSplashScreen
{
    public string AppName { get; }
    public IImage AppIcon { get; }
    public  MainWindow _owner = owner;
    
    public static Window? _mainWindow = ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!)?.MainWindow;
    public object SplashScreenContent => new LoadingWindow();
    public int MinimumShowTime => 3000;

    private static Action InitApp =>
        () =>
        {
            var validGtaPath = "";
            if (string.IsNullOrEmpty(Program.ArbolitoSettings?._ArbolitoSettings.gtapath))
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    IMsBox<ButtonResult> box = MessageBoxManager
                        .GetMessageBoxStandard("Warning",
                            "Arbolito couldn't find your GTA V folder. Now it will try to detect it automatically.",
                            ButtonEnum.Ok);
                    ButtonResult result = await box.ShowWindowDialogAsync(_mainWindow);
                }).Wait();

                var steamGtavPath = Directory.GetParent(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Rockstar Games\GTAV",
                    "InstallFolderSteam", null)?.ToString());
                var normalGtavPath = Registry
                    .GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Rockstar Games\Grand Theft Auto V",
                        "InstallFolder", null)?.ToString();
                var exeExists = false;

                if (steamGtavPath != null)
                {
                    exeExists = File.Exists(steamGtavPath + "\\GTA5.exe");
                    validGtaPath = steamGtavPath.ToString();
                }

                if (normalGtavPath != null && !exeExists)
                {
                    exeExists = File.Exists(normalGtavPath + "\\GTA5.exe");
                    validGtaPath = normalGtavPath;
                }

                if (exeExists)
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var box = MessageBoxManager
                            .GetMessageBoxStandard("Success", $"Arbolito found your GTA V folder. \n{validGtaPath}\nIs this correct?",
                                ButtonEnum.YesNo);
                        var result = await box.ShowAsync();

                        if (result == ButtonResult.Yes)
                        {
                            Program.ArbolitoSettings._ArbolitoSettings.gtapath = validGtaPath;
                            Program.SaveJsonSettings();
                        }
                        else
                        {
                            Dispatcher.UIThread.InvokeAsync(async () =>
                            {
                                var box2 = MessageBoxManager
                                    .GetMessageBoxStandard("Informatioon", "Select your GTA path.",
                                        ButtonEnum.Ok);
                                await box2.ShowAsync();
                            }).Wait();
                        }
                    }).Wait();
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var box = MessageBoxManager
                            .GetMessageBoxStandard("Error", "Arbolito couldn't find your GTA V folder, select your path.",
                                ButtonEnum.Ok);
                        await box.ShowAsync();
                        
                        var folder = await _mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
                        {
                            AllowMultiple = false,
                            Title = "Select your GTA V folder"
                        }); 
                        
                        if (folder.Any())
                        {
                            Program.ArbolitoSettings._ArbolitoSettings.gtapath = folder[0].Path.LocalPath;
                        }
                        
                    }).Wait();
                }
            }

            //wait for the user to select the GTA folder
        };

    public Task RunTasks(CancellationToken cancellationToken)
    {
        return InitApp == null ? Task.CompletedTask : Task.Run(InitApp, cancellationToken);
    }
}