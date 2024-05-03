using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Commands.Helpers;
public static class ActionIconResolverHelper
{
    private static readonly string iconsBaseDirectory = 
        Path.Combine(AppContext.BaseDirectory, "Assets", "CommandStepIcons");

    public static ImageSource? GetIconByName(string pluginName)
    {
        var uri = new Uri(Path.Combine(iconsBaseDirectory, $"{pluginName}.png"));
        return uri != null ? new BitmapImage(uri) : null;
    }
}
