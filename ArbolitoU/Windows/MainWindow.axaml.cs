using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Avalonia.Styling;
using Avalonia.Threading;
using CodeWalker.GameFiles;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Windowing;
using Microsoft.Win32;

namespace ArbolitoU;

public partial class MainWindow : AppWindow
{
    public static GameFileCache gameFileCache;
    public static FileManager fm = new FileManager();

    public MainWindow()
    {
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        InitializeComponent();

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

    private void MiReportIssue_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        //Open Github issues page


        Process.Start(@"https://github.com/Hancapo/ArbolitoU/issues");

    }

    private void MenuItemOpenFiles_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var openFiles = this.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select game assets",
            AllowMultiple = true,
            FileTypeFilter = fm.GetSupportedFilesFilter()
        });

    }

    private void MenuItemAbout_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        FileSelection fs = new();
        fs.ShowDialog(this);
    }
}

internal class ArbolitoSplashScreen(MainWindow owner) : IApplicationSplashScreen
{
    public string AppName { get; }
    public IImage AppIcon { get; }

    public static Window? _mainWindow =
        ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime!)?.MainWindow;

    public static LoadingWindow SplashScreen = new();
    public object SplashScreenContent => SplashScreen;
    public int MinimumShowTime => 3000;

    private static void UpdateStatusCache(string obj)
    {
        Dispatcher.UIThread.InvokeAsync(() => { SplashScreen.tbLoadingStatus.Text = obj; });
    }

    private static Action InitApp =>
        () =>
        {
            var validGtaPath = "";
            if (string.IsNullOrEmpty(Program.ArbolitoSettings?.CurrentSettings.gtapath))
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var warningDialog = new ContentDialog()
                    {
                        Title = "Warning",
                        Content =
                            "Arbolito couldn't find your GTA V folder. \nNow it will try to detect it automatically.",
                        PrimaryButtonText = "Ok",
                        DefaultButton = ContentDialogButton.Primary
                    };
                    await warningDialog.ShowAsync();
                }).Wait();
                string? steamGTAVReg = (string?)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Rockstar Games\GTAV",
                        "InstallFolderSteam", null);
                string? normalGTAVReg = (string?)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Rockstar Games\Grand Theft Auto V",
                    "InstallFolder", null);

                var steamGTAVpath = "";
                var normalGTAVPath = "";

                if (steamGTAVReg != null)
                {
                    steamGTAVpath = Directory.GetParent(steamGTAVReg).FullName;
                }
                if (normalGTAVReg != null)
                {
                    normalGTAVPath = normalGTAVReg?.ToString();
                }


                var exeExists = false;

                if (!string.IsNullOrEmpty(steamGTAVpath))
                {
                    exeExists = File.Exists(steamGTAVpath + "\\GTA5.exe");
                    validGtaPath = steamGTAVpath.ToString();
                }

                if (!string.IsNullOrEmpty(normalGTAVPath) && !exeExists)
                {
                    exeExists = File.Exists(normalGTAVPath + "\\GTA5.exe");
                    validGtaPath = normalGTAVPath;
                }

                if (exeExists)
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var successDialog = new ContentDialog()
                        {
                            Title = "Success",
                            Content = $"Arbolito has found your GTA V folder. \n{validGtaPath}\nIs this correct?",
                            PrimaryButtonText = "Yes",
                            SecondaryButtonText = "No",
                            DefaultButton = ContentDialogButton.Primary,
                        };
                        var arbFoundResult = await successDialog.ShowAsync();

                        if (arbFoundResult == ContentDialogResult.Primary)
                        {
                            ArbolitoSettings arbolitoSettings2 = new();
                            if (Program.ArbolitoSettings is null)
                            {
                                arbolitoSettings2 = new ArbolitoSettings
                                {
                                    CurrentSettings = new SettingsContainer()
                                    {
                                        gtapath = validGtaPath
                                    }
                                };
                            }

                            Program.ArbolitoSettings = arbolitoSettings2;
                            Program.SaveJsonSettings();
                        }
                        else
                        {
                            var infoDialog = new ContentDialog()
                            {
                                Title = "Information",
                                Content = "Please select your GTA path.",
                                PrimaryButtonText = "Ok"
                            };
                            var noFoundManuallyResult = await infoDialog.ShowAsync();
                            if (noFoundManuallyResult == ContentDialogResult.Primary)
                            {
                                var selectedPath = "";
                                var validGtaPathB = false;
                                while (!validGtaPathB)
                                {
                                    var manualFolder = await _mainWindow!.StorageProvider.OpenFolderPickerAsync(
                                        new FolderPickerOpenOptions()
                                        {
                                            AllowMultiple = false,
                                            Title = "Select your GTA V folder"
                                        });
                                    if (!manualFolder.Any()) continue;
                                    selectedPath = manualFolder[0]?.Path.LocalPath;
                                    validGtaPathB = File.Exists(selectedPath + "\\GTA5.exe");
                                }

                                ArbolitoSettings arbolitoSettings3 = new();
                                if (Program.ArbolitoSettings is null)
                                {
                                    arbolitoSettings3 = new ArbolitoSettings
                                    {
                                        CurrentSettings = new SettingsContainer()
                                        {
                                            gtapath = selectedPath
                                        }
                                    };
                                }

                                Program.ArbolitoSettings = arbolitoSettings3;
                            }
                        }
                    }).Wait();
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        var errorDialog = new ContentDialog()
                        {
                            Title = "Error",
                            Content = "Arbolito couldn't find your GTA V folder, select your path.",
                            PrimaryButtonText = "Ok"
                        };
                        await errorDialog.ShowAsync();
                    }).Wait();

                    var folderGame = _mainWindow?.StorageProvider.OpenFolderPickerAsync(
                        new FolderPickerOpenOptions()
                        {
                            AllowMultiple = false,
                            Title = "Select your GTA V folder",
                        }
                        );

                    if (folderGame != null && folderGame.Result.Any())
                    {
                        if (Program.ArbolitoSettings is null)
                            Program.ArbolitoSettings = new ArbolitoSettings()
                            {
                                CurrentSettings = new SettingsContainer()
                                {
                                    gtapath = folderGame.Result[0].Path.LocalPath
                                }
                            };

                    }
                    else
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            var noFolderSelectedDialog = new ContentDialog()
                            {
                                Title = "Error",
                                Content = "You didn't select a folder, Arbolito will close now.",
                                PrimaryButtonText = "Ok",
                                DefaultButton = ContentDialogButton.Primary
                            };
                            await noFolderSelectedDialog.ShowAsync();

                        }).Wait();
                        Environment.Exit(0);
                    }

                }
            }

            if (string.IsNullOrEmpty(Program.ArbolitoSettings?.CurrentSettings.outputpath))
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var outputDialog = new ContentDialog()
                    {
                        Title = "Warning",
                        Content = "The output path is not set. Please set it now.",
                        PrimaryButtonText = "Ok",
                        DefaultButton = ContentDialogButton.Primary
                    };
                    await outputDialog.ShowAsync();

                    var folderOutput = _mainWindow?.StorageProvider.OpenFolderPickerAsync(
                        new FolderPickerOpenOptions()
                        {
                            AllowMultiple = false,
                            Title = "Select your output folder"
                        });

                    if (folderOutput != null && folderOutput.Result.Any())
                    {
                        Program.ArbolitoSettings.CurrentSettings.outputpath = folderOutput.Result[0].Path.LocalPath;
                        Program.SaveJsonSettings();
                    }
                    else
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            var noFolderSelectedDialog = new ContentDialog()
                            {
                                Title = "Error",
                                Content = "You didn't select an output folder, Arbolito will close now.",
                                PrimaryButtonText = "Ok",
                                DefaultButton = ContentDialogButton.Primary
                            };
                            await noFolderSelectedDialog.ShowAsync();
                        }).Wait();
                        Environment.Exit(0);
                    }
                }).Wait();
            }




            GTA5Keys.LoadFromPath(Program.ArbolitoSettings.CurrentSettings.gtapath);
            MainWindow.gameFileCache = new GameFileCache(2147483648, 10,
                Program.ArbolitoSettings.CurrentSettings.gtapath, null, false,
                "Installers;_CommonRedist");
            Task.Run(() => MainWindow.gameFileCache.Init(UpdateStatusCache, UpdateStatusCache)).Wait();
        };


    public Task RunTasks(CancellationToken cancellationToken)
    {
        return InitApp == null ? Task.CompletedTask : Task.Run(InitApp, cancellationToken);
    }
}