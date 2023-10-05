using System.Collections.Generic;
using ArbolitoU.Pages;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Windowing;

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
        HomeItem.IsSelected = true;
    }
    
    private void HomeItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        
        if(ArbolitoFrame.Content?.GetType() != typeof(Home))
            ArbolitoFrame.Navigate(typeof(Home), null, new DrillInNavigationTransitionInfo());
    }

    private void FileExtItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if(ArbolitoFrame.Content?.GetType() != typeof(FileExtractor))
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
            FileTypeFilter = new[] {new FilePickerFileType("YMAP(s)"){Patterns = new[] {"*.ymap"}}}
        });
        
    }

    private void MiSelectYTYP_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var ytypSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select YTYP(s) file(s)",
            AllowMultiple = true,
            FileTypeFilter = new[] {new FilePickerFileType("YTYP(s)"){Patterns = new[] {"*.ytyp"}}}
        });
    }

    private void MiSelectTrainTracks_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var trainTracksSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select Train Tracks file(s)",
            AllowMultiple = true,
            FileTypeFilter = new[] {new FilePickerFileType("Train Tracks"){Patterns = new[] {"*.dat"}}}
        });
    }

    private void MiSelectTextFile_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var textFileSelection = GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select Text file",
            AllowMultiple = false,
            FileTypeFilter = new[] {new FilePickerFileType("Text File"){Patterns = new[] {"*.txt"}}}
        });
    }

    private void SettingsItem_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if(ArbolitoFrame.Content?.GetType() != typeof(Settings))
            ArbolitoFrame.Navigate(typeof(Settings), null, new DrillInNavigationTransitionInfo());
    }
}