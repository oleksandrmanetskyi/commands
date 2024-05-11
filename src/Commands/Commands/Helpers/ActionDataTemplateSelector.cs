using Commands.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Commands.Helpers;
public class ActionDataTemplateSelector: DataTemplateSelector
{
    public DataTemplate CommandLineTemplate { get; set; }
    public DataTemplate TextInputTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is Core.Models.Action action)
        {
            return action.Layout switch
            {
                Layouts.CommandLine => CommandLineTemplate,
                Layouts.TextInput => TextInputTemplate,
                _ => throw new NotImplementedException(),
            };
        }

        return base.SelectTemplateCore(item);
    }
}
