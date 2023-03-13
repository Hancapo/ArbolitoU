using System.Collections.Generic;
using System.Windows.Controls;

namespace ArbolitoU.Pages.MapTools.YMAP;

public partial class YmapSplitter : Page
{
    public YmapSplitter()
    {
        InitializeComponent();
        CbSeparateBy.ItemsSource = new List<string> {"YTYPs", "Text file"};
    }
}