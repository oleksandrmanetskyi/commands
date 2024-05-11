using Microsoft.UI.Xaml.Controls;

namespace Commands.Controls;

public sealed partial class ActionHeaderControl : UserControl
{
    public string PluginName { get; set; } = string.Empty;

    public ActionHeaderControl()
    {
        this.InitializeComponent();
    }
}
