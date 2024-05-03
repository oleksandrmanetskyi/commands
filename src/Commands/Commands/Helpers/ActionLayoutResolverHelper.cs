using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands.Controls;
using Commands.Core;
using Microsoft.UI.Xaml.Controls;

namespace Commands.Helpers;
public class ActionLayoutResolverHelper
{
    public static UserControl ResolveLayout(Layouts pluginName, Dictionary<string, string> parameters)
    {
        return pluginName switch
        {
            Layouts.CommandLine => new CommandLineActionLayoutContent(parameters),
            Layouts.TextInput => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }
}
