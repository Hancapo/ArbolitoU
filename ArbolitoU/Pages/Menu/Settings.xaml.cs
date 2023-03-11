using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using ArbolitoU.UI;
using ArbolitoU.Utils;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ArbolitoU.Pages.Menu;

public partial class Settings : Page
{
    public Settings()
    {
        InitializeComponent();
        LoadSettingsFromParent();
    }

    private void RbLight_OnChecked(object sender, RoutedEventArgs e)
    {
        Theme.Apply(ThemeType.Light);
    }

    private void RbDark_OnChecked(object sender, RoutedEventArgs e)
    {
        Theme.Apply(ThemeType.Dark);
    }
    
    void GetSelectedTheme()
    {
        if (Theme.GetAppTheme() == ThemeType.Light)
        {
            rbLight.IsChecked = true;
        }
        else
        {
            rbDark.IsChecked = true;
        }
    }

    private void LoadSettingsFromParent()
    {
        var parent = (MainWindow) Application.Current.MainWindow; 
        tbOutputFolder.Text = parent.OutputFolder;
        tbGTAVPath.Text = parent.GTAVPath;
        cbEnableMods.IsChecked = parent.EnableMods;
        cbEnableDLCs.IsChecked = parent.EnableDLCs;
        GetSelectedTheme();
    }

    private void BtnSaveSettings_OnClick(object sender, RoutedEventArgs e)
    {
        SaveSettings();
    }

    private void BtnBrowseOutput_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog outputDialog = new(){Multiselect = false, Description = "Select output folder for all the generated files.", ShowNewFolderButton = true, UseDescriptionForTitle = true};
        if (outputDialog.ShowDialog() == true)
        {
            tbOutputFolder.Text = outputDialog.SelectedPath;
        }
    }

    private void BtnBrowseGTAVPath_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog gtavDialog = new(){Multiselect = false, Description = "Select your GTAV folder.", ShowNewFolderButton = true, UseDescriptionForTitle = true};
        if (gtavDialog.ShowDialog() == true)
        {
            if(File.Exists(gtavDialog.SelectedPath + "\\GTA5.exe"))
            {
                SimpleFluentMessageBox GTAVFound = new("Game found", 
                    "GTAV.exe found in the selected folder.", 
                    "Accept", 
                    "Cancel", 
                    ControlAppearance.Success, 
                    ControlAppearance.Secondary);
                SystemSounds.Hand.Play();
                GTAVFound.ShowDialog();
                tbGTAVPath.Text = gtavDialog.SelectedPath;
            }
            else
            {
                SimpleFluentMessageBox NoGTAerror = new("Error", 
                    "The selected folder doesn't contain GTA5.exe. Please select a valid \nGTAV folder.", 
                    "Accept", 
                    "Cancel", 
                    ControlAppearance.Danger, 
                    ControlAppearance.Secondary);
                SystemSounds.Exclamation.Play();
                NoGTAerror.ShowDialog();
            }
        }
    }

    private void BtnDefaultSettings_OnClick(object sender, RoutedEventArgs e)
    {
        SimpleFluentMessageBox DefaultMsg = new("Warning", 
            "Are you sure you want to reset all the settings to default?\nThis action will also delete the configuration file.", 
            "Yes", 
            "No", 
            ControlAppearance.Danger, 
            ControlAppearance.Secondary);
        SystemSounds.Exclamation.Play();
        if (DefaultMsg.ShowDialog() == true)
        {
            ResetSettings();
        }
    }
    
    void ResetSettings()
    {
        tbOutputFolder.Text = "";
        tbGTAVPath.Text = "";
        cbEnableMods.IsChecked = false;
        cbEnableDLCs.IsChecked = false;
        rbLight.IsChecked = false;
        rbDark.IsChecked = true;
        Theme.Apply(ThemeType.Dark);
        File.Delete("./settings.ini");
        var parent = (MainWindow) Application.Current.MainWindow; 
        parent.OutputFolder = string.Empty;
        parent.GTAVPath = string.Empty;
        parent.EnableMods = false;
        parent.EnableDLCs = false;
        
    }
    void SaveSettings()
    {
        if (tbOutputFolder.Text != string.Empty && Directory.Exists(tbOutputFolder.Text) && tbGTAVPath.Text != string.Empty)
        {
            SimpleFluentMessageBox AreYouSure = new("Warning", 
                "Are you sure you want to save the settings?", 
                "Yes", 
                "No", 
                ControlAppearance.Danger, 
                ControlAppearance.Secondary);
            SystemSounds.Hand.Play();
            if (AreYouSure.ShowDialog())
            {
                var parent = (MainWindow) Application.Current.MainWindow; 
                parent.OutputFolder = tbOutputFolder.Text;
                parent.GTAVPath = tbGTAVPath.Text;
                parent.EnableMods = (bool) cbEnableMods.IsChecked;
                parent.EnableDLCs = (bool) cbEnableDLCs.IsChecked;
        
                var settings = new IniFile("./settings.ini");
                settings.WriteValue("Settings", "Theme", Theme.GetAppTheme().ToString());
                settings.WriteValue("Paths", "OutputFolder", tbOutputFolder.Text);
                settings.WriteValue("Paths", "GTAVPath", tbGTAVPath.Text);
                settings.WriteValue("RPFLoading", "EnableMods", cbEnableMods.IsChecked.ToString());
                settings.WriteValue("RPFLoading", "EnableDLC", cbEnableDLCs.IsChecked.ToString());
                
                SimpleFluentMessageBox Saved = new("Success", 
                    "Settings saved successfully.", 
                    "Accept", 
                    "Cancel", 
                    ControlAppearance.Success, 
                    ControlAppearance.Secondary);
                
                SystemSounds.Hand.Play();
                Saved.ShowDialog();
            }
        }
        else
        {
            SimpleFluentMessageBox CantSave = new("Error", 
                "You must select a valid output folder and GTAV folder before saving \nsettings.", 
                "Accept", 
                "Cancel", 
                ControlAppearance.Danger, 
                ControlAppearance.Secondary);
            SystemSounds.Exclamation.Play();
            CantSave.ShowDialog();
        }

        

        
    }
}   