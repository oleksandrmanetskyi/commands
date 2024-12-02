using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Commands.Helpers;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool boolValue;
        if (value is bool v)
        {
            boolValue = v;
        }
        else if (value is int intValue)
        {
            boolValue = intValue > 0;
        }
        else
        {
            return Visibility.Collapsed;
        }

        if (parameter is string stringParam && stringParam.Equals("invert"))
        {
            boolValue = !boolValue;
        }
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
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
