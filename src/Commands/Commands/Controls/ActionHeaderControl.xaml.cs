using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Commands.Controls;

public sealed partial class ActionHeaderControl : UserControl
{
    public string PluginName { get; set; } = string.Empty;

    public static readonly DependencyProperty ContentInsideProperty =
            DependencyProperty.Register("ContentInside", typeof(UIElement), typeof(ActionHeaderControl), new PropertyMetadata(null));

    public UIElement ContentInside
    {
        get => (UIElement)GetValue(ContentInsideProperty);
        set => SetValue(ContentInsideProperty, value);
    }

    public ActionHeaderControl()
    {
        this.InitializeComponent();
    }
}
