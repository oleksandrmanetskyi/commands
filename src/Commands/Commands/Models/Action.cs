using Microsoft.UI.Xaml.Controls;

namespace Commands.Models;
public class ActionWithLayout : Core.Models.Action
{
    public required Page Layout { get; set; }
}
