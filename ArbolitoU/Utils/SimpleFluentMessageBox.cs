using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wpf.Ui.Controls;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ArbolitoU.Utils;

public class SimpleFluentMessageBox
{
    public SimpleFluentMessageBox(string title, string message, string leftButtonString, string rightButtonString,
        ControlAppearance LButton, ControlAppearance RButton)
    {
        mb = new MessageBox
        {
            Title = title,
            Content = message,
            ButtonLeftName = leftButtonString,
            ButtonRightName = rightButtonString,
            ResizeMode = ResizeMode.NoResize,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MicaEnabled = true,
            ButtonLeftAppearance = LButton,
            ButtonRightAppearance = RButton,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
            
        };
        mb.ButtonLeftClick += MbLButtonClick;
        mb.ButtonRightClick += MbRButtonClick;
    }

    public SimpleFluentMessageBox(string title, string message)
    {
        mb = new MessageBox
        {
            Title = title,
            Content = message,
            ButtonLeftName = "Accept",
            ButtonRightName = "Cancel",
            ResizeMode = ResizeMode.NoResize,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            MicaEnabled = true,
            VerticalContentAlignment = VerticalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        mb.ButtonLeftClick += MbLButtonClick;
        mb.ButtonRightClick += MbRButtonClick;
    }
    private bool? LButtonPressed { get; set; }
    private bool? RButtonPressed { get; set; }
    private static MessageBox? mb { get; set; }

    private void MbLButtonClick(object sender, RoutedEventArgs e)
    {
        LButtonPressed = true;
        RButtonPressed = false;
        (sender as MessageBox)?.Close();
    }

    private void MbRButtonClick(object sender, RoutedEventArgs e)
    {
        LButtonPressed = false;
        RButtonPressed = true;
        (sender as MessageBox)?.Close();
    }


    public bool ShowDialog()
    {
        mb.ShowDialog();
        try
        {
            if ((bool)LButtonPressed) return true;
            return (bool)RButtonPressed && false;
        }
        catch (Exception)
        {
        }
        return false;
    }
}