using Avalonia;
using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Configuration;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;

namespace ArbolitoU;

class Program
{
    internal static IConfigurationRoot? Configuration;
    internal static ArbolitoSettings? ArbolitoSettings = new();
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange:true);
        
        Configuration = configuration.Build();
        ArbolitoSettings = Configuration.Get<ArbolitoSettings>();

        
        IconProvider.Current
            .Register<MaterialDesignIconProvider>();
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }
    
    public static void SaveJsonSettings()
    {
        if(ArbolitoSettings is null) return;
        var jsonConfig = JsonSerializer.Serialize(ArbolitoSettings, new JsonSerializerOptions{WriteIndented = true });
        File.WriteAllText("appsettings.json", jsonConfig);
    }
    
    public static void ResetJsonSettings()
    {
        File.Delete("appsettings.json");
    }

    
    
}