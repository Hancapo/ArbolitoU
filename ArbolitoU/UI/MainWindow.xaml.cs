using System.Windows;
using ArbolitoU.Utils;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Window;

namespace ArbolitoU.UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
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
}