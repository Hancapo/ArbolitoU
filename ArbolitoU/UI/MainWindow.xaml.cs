using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using ArbolitoU.Pages.Menu;
using ArbolitoU.Utils;
using Ookii.Dialogs.Wpf;
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
    
    public List<string> YmapFiles { get; set; }
    public List<string> YtypFiles { get; set; }
    public List<string> YnvFiles { get; set; }
    public List<string> TrainsFiles { get; set; }
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectYmapClick(object sender, RoutedEventArgs e)
    {
        var ymapDialog = new VistaFolderBrowserDialog
        {
            Description = "Select the folder containing the YMAP files",
            UseDescriptionForTitle = true,
            Multiselect = false,
        };

        if (ymapDialog.ShowDialog() != true) return;
        if (Directory.GetFiles(ymapDialog.SelectedPath, "*.ymap").Length == 0)
        {
            new SimpleFluentMessageBox("Error", "No YMAP files found in the selected folder", "Accept", "Cancel", ControlAppearance.Danger, ControlAppearance.Primary).ShowDialog();
        }
        else
        {
            YmapFiles = new List<string>(Directory.GetFiles(ymapDialog.SelectedPath, "*.ymap"));
        }
    }


    private void ExitClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ArbolitoNavigation_OnLoaded(object sender, RoutedEventArgs e)
    {
        ArbolitoNavigation.Navigate(typeof(Home));
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        //Load settings from ini file if it exists and is valid
        if (!File.Exists("./settings.ini")) return;
        var settings = new IniFile("./settings.ini");
        Theme.Apply(settings.ReadValue("Settings", "Theme") == "Dark" ? ThemeType.Dark : ThemeType.Light);
        OutputFolder = settings.ReadValue("Paths", "OutputFolder");
        GTAVPath = settings.ReadValue("Paths", "GTAVPath");
        EnableMods = Convert.ToBoolean(settings.ReadValue("RPFLoading", "EnableMods"));
        EnableDLCs = Convert.ToBoolean(settings.ReadValue("RPFLoading", "EnableDLC"));

    }

    private void SelectYtypClick(object sender, RoutedEventArgs e)
    {
        var ytypDialog = new VistaFolderBrowserDialog
        {
            Description = "Select the folder containing the YTYP files",
            UseDescriptionForTitle = true,
            Multiselect = false,
        };

        if(ytypDialog.ShowDialog() != true) return;
        if (Directory.GetFiles(ytypDialog.SelectedPath, "*.ytyp").Length == 0)
        {
            new SimpleFluentMessageBox("Error", "No YTYP files found in the selected folder.", "Accept", "Cancel", ControlAppearance.Danger, ControlAppearance.Primary).ShowDialog();
        }
        else
        {
            YtypFiles = new List<string>(Directory.GetFiles(ytypDialog.SelectedPath, "*.ytyp"));
        }
    }

    private void SelectTrainsClick(object sender, RoutedEventArgs e)
    {
        var trainsDialog = new VistaFolderBrowserDialog
        {
            Description = "Select the folder containing the Train Tracks files",
            UseDescriptionForTitle = true,
            Multiselect = false,
        };

        if (trainsDialog.ShowDialog() != true) return;
        if (Directory.GetFiles(trainsDialog.SelectedPath, "*.dat").Length == 0)
        {
            new SimpleFluentMessageBox("Error", "No Train Tracks files found in the selected folder.", "Accept", "Cancel", ControlAppearance.Danger, ControlAppearance.Primary).ShowDialog();
        }
        else
        {
            TrainsFiles = new List<string>(Directory.GetFiles(trainsDialog.SelectedPath, "*.dat"));
        }
    }

    private void SelectYnvClick(object sender, RoutedEventArgs e)
    {
        var ynvDialog = new VistaFolderBrowserDialog
        {
            Description = "Select the folder containing the YNV files",
            UseDescriptionForTitle = true,
            Multiselect = false,
        };

        if (ynvDialog.ShowDialog() != true) return;
        if (Directory.GetFiles(ynvDialog.SelectedPath, "*.ynv").Length == 0)
        {
            new SimpleFluentMessageBox("Error", "No YNV files found in the selected folder.", "Accept", "Cancel", ControlAppearance.Danger, ControlAppearance.Primary).ShowDialog();
        }
        else
        {
            YnvFiles = new List<string>(Directory.GetFiles(ynvDialog.SelectedPath, "*.ynv"));
        }
    }
}