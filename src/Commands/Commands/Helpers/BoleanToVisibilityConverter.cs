using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Commands.Helpers;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            if (parameter is string stringParam && stringParam.Equals("invert"))
            {
                boolValue = !boolValue;
            }
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var result = false;
        if (value is Visibility visibility)
        {
            result = visibility == Visibility.Visible;
        }

        if (parameter is string stringParam && stringParam.Equals("invert"))
        {
            result = !result;
        }

        return result;
    }
}
