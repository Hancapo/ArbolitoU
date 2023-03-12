using System.Collections.Generic;
using System.Windows.Controls;

namespace ArbolitoU.Pages.Map_Tools.YMAP;

public partial class YmapSplitter : Page
{
    public YmapSplitter()
    {
        InitializeComponent();
        cbSeparateBy.ItemsSource = new List<string> {"YTYPs", "Text file"};
    }
}