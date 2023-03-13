using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace ArbolitoU.Utils;

public class SimpleSnackback
{
    public SimpleSnackback()
    {
        //Will work on it later, not needed right now.
        var snack = new Snackbar
        {
            Icon = SymbolRegular.Subtitles16
        }.Show();
    }
}