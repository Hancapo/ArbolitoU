using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ArbolitoU.Pages.Menu;
using ArbolitoU.Utils;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Controls.Window;
using MessageBox = System.Windows.MessageBox;

namespace ArbolitoU.UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    //Set global variables for the app such as Theme, Output folder, GTAV path, etc.
    public string OutputFolder { get; set; }
    public string GTAVPath { get; set; }
    public bool EnableMods { get; set; }
    public bool EnableDLCs { get; set; }
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectYmapClick(object sender, RoutedEventArgs e)
    {
        var msgBox = new SimpleFluentMessageBox("File Selection",
                "Please select your file\nMis amigos",
                "Accept",
                "Cancel",
                ControlAppearance.Transparent,
                ControlAppearance.Info)
            .ShowDialog();
    }


    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ArbolitoNavigation_OnLoaded(object sender, RoutedEventArgs e)
    {
        var ArbolitoNav = (sender as NavigationView);
        ArbolitoNav.Navigate(typeof(Home));
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        //Load settings from ini file if it exists and is valid
        if (File.Exists("./settings.ini"))
        {
            var settings = new IniFile("./settings.ini");
            Theme.Apply(settings.ReadValue("Settings", "Theme") == "Dark" ? ThemeType.Dark : ThemeType.Light);
            OutputFolder = settings.ReadValue("Paths", "OutputFolder");
            GTAVPath = settings.ReadValue("Paths", "GTAVPath");
            EnableMods = Convert.ToBoolean(settings.ReadValue("RPFLoading", "EnableMods"));
            EnableDLCs = Convert.ToBoolean(settings.ReadValue("RPFLoading", "EnableDLC"));
        }
        
    }
}