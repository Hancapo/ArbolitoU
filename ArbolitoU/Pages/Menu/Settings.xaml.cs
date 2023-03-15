using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using ArbolitoU.UI;
using ArbolitoU.Utils;
using Ookii.Dialogs.Wpf;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ArbolitoU.Pages.Menu;

public partial class Settings : Page
{
    private readonly MainWindow parent = (MainWindow)Application.Current.MainWindow;

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

    private void GetSelectedTheme()
    {
        if (Theme.GetAppTheme() == ThemeType.Light)
        {
            RbLight.IsChecked = true;
        }
        else
        {
            RbDark.IsChecked = true;
        }
    }

    private void LoadSettingsFromParent()
    {
        TbOutputFolder.Text = parent.OutputFolder;
        TbGtavPath.Text = parent.GTAVPath;
        CbEnableMods.IsChecked = parent.EnableMods;
        CbEnableDlCs.IsChecked = parent.EnableDLCs;
        GetSelectedTheme();
    }

    private void BtnSaveSettings_OnClick(object sender, RoutedEventArgs e)
    {
        SaveSettings();
    }

    private void BtnBrowseOutput_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog outputDialog = new()
        {
            Multiselect = false,
            Description = "Select output folder for all the generated files.",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        if (outputDialog.ShowDialog() == true)
        {
            TbOutputFolder.Text = outputDialog.SelectedPath;
        }
    }

    private void BtnBrowseGTAVPath_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog gtavDialog = new()
        {
            Multiselect = false,
            Description = "Select your GTAV folder.",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        if (gtavDialog.ShowDialog() == true)
        {
            if (File.Exists(gtavDialog.SelectedPath + "\\GTA5.exe"))
            {
                SimpleFluentMessageBox gtavFound = new("Game found", "GTAV.exe found in the selected folder.", "Accept",
                    "Cancel", ControlAppearance.Success, ControlAppearance.Secondary);
                SystemSounds.Hand.Play();
                gtavFound.ShowDialog();
                TbGtavPath.Text = gtavDialog.SelectedPath;
            }
            else
            {
                SimpleFluentMessageBox noGTAerror = new("Error",
                    "The selected folder doesn't contain GTA5.exe. Please select a valid \nGTAV folder.", "Accept",
                    "Cancel", ControlAppearance.Danger, ControlAppearance.Secondary);
                SystemSounds.Exclamation.Play();
                noGTAerror.ShowDialog();
            }
        }
    }

    private void BtnDefaultSettings_OnClick(object sender, RoutedEventArgs e)
    {
        SimpleFluentMessageBox defaultMsg = new("Warning",
            "Are you sure you want to reset all the settings to default?\nThis action will also delete the current configuration file.",
            "Yes", "No", ControlAppearance.Danger, ControlAppearance.Secondary);
        SystemSounds.Exclamation.Play();
        if (defaultMsg.ShowDialog())
        {
            ResetSettings();
        }
    }

    private void ResetSettings()
    {
        TbOutputFolder.Text = "";
        TbGtavPath.Text = "";
        CbEnableMods.IsChecked = false;
        CbEnableDlCs.IsChecked = false;
        RbLight.IsChecked = false;
        RbDark.IsChecked = true;
        Theme.Apply(ThemeType.Dark);
        File.Delete("./settings.ini");
        parent.OutputFolder = string.Empty;
        parent.GTAVPath = string.Empty;
        parent.EnableMods = false;
        parent.EnableDLCs = false;
    }

    private void SaveSettings()
    {
        if (TbOutputFolder.Text != string.Empty && Directory.Exists(TbOutputFolder.Text) &&
            TbGtavPath.Text != string.Empty)
        {
            SimpleFluentMessageBox areYouSure = new("Warning", "Are you sure you want to save the settings?", "Yes",
                "No", ControlAppearance.Danger, ControlAppearance.Secondary);
            SystemSounds.Hand.Play();
            if (areYouSure.ShowDialog())
            {
                parent.OutputFolder = TbOutputFolder.Text;
                parent.GTAVPath = TbGtavPath.Text;
                parent.EnableMods = (bool)CbEnableMods.IsChecked;
                parent.EnableDLCs = (bool)CbEnableDlCs.IsChecked;

                var settings = new IniFile("./settings.ini");
                settings.WriteValue("Settings", "Theme", Theme.GetAppTheme().ToString());
                settings.WriteValue("Paths", "OutputFolder", TbOutputFolder.Text);
                settings.WriteValue("Paths", "GTAVPath", TbGtavPath.Text);
                settings.WriteValue("RPFLoading", "EnableMods", CbEnableMods.IsChecked.ToString());
                settings.WriteValue("RPFLoading", "EnableDLC", CbEnableDlCs.IsChecked.ToString());

                SimpleFluentMessageBox saved = new("Success", "Settings saved successfully.", "Accept", "Cancel",
                    ControlAppearance.Success, ControlAppearance.Secondary);

                SystemSounds.Hand.Play();
                saved.ShowDialog();
            }
        }
        else
        {
            SimpleFluentMessageBox cantSave = new("Error",
                "You must select a valid output folder and GTAV folder before\nsaving settings.", "Accept", "Cancel",
                ControlAppearance.Danger, ControlAppearance.Secondary);
            SystemSounds.Exclamation.Play();
            cantSave.ShowDialog();
        }
    }

    private void BtnBrowseFXServer_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog fxserverDialog = new()
        {
            Multiselect = false,
            Description = "Select your FXServer folder.",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        
        if (fxserverDialog.ShowDialog() == true)
        {
            if (File.Exists(fxserverDialog.SelectedPath + "\\FXServer.exe"))
            {
                SimpleFluentMessageBox fxserverFound = new("Information", "FXServer.exe was found in the selected folder.", "Accept",
                    "Cancel", ControlAppearance.Success, ControlAppearance.Secondary);
                SystemSounds.Hand.Play();
                fxserverFound.ShowDialog();
                TbFXServer.Text = fxserverDialog.SelectedPath;
            }
            else
            {
                SimpleFluentMessageBox noFxServererror = new("Error",
                    "The selected folder doesn't contain FXServer.exe. Please select a valid \nFXServer folder.", "Accept",
                    "Cancel", ControlAppearance.Danger, ControlAppearance.Secondary);
                SystemSounds.Exclamation.Play();
                noFxServererror.ShowDialog();
            }
        }
    }

    private void BtnBrowseFiveM_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog fivemDialog = new()
        {
            Multiselect = false,
            Description = "Select your FiveM folder.",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        
        if (fivemDialog.ShowDialog() == true)
        {
            if (File.Exists(fivemDialog.SelectedPath + "\\FiveM.exe"))
            {
                SimpleFluentMessageBox fivemFound = new("Information", "FiveM.exe found in the selected folder.", "Accept",
                    "Cancel", ControlAppearance.Success, ControlAppearance.Secondary);
                SystemSounds.Hand.Play();
                fivemFound.ShowDialog();
                TbFiveM.Text = fivemDialog.SelectedPath;
            }
            else
            {
                SimpleFluentMessageBox noFiveMerror = new("Error",
                    "The selected folder doesn't contain FiveM.exe. Please select a valid \nFiveM folder.", "Accept",
                    "Cancel", ControlAppearance.Danger, ControlAppearance.Secondary);
                SystemSounds.Exclamation.Play();
                noFiveMerror.ShowDialog();
            }
        }
        
    }

    private void BtnResourceOut_OnClick(object sender, RoutedEventArgs e)
    {
        VistaFolderBrowserDialog resourceDialog = new()
        {
            Multiselect = false,
            Description = "Select your resource folder.",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true
        };
        
        if (resourceDialog.ShowDialog() == true)
        {
            SimpleFluentMessageBox resourceFound = new("Information", "Selected valid resource folder.", "Accept",
                "Cancel", ControlAppearance.Success, ControlAppearance.Secondary);
            SystemSounds.Hand.Play();
            resourceFound.ShowDialog();
            TbResources.Text = resourceDialog.SelectedPath;
        }
    }
}