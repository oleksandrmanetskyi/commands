using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands.Core.ActionPlugins;
using Commands.Core.Models;
using Microsoft.UI.Xaml.Controls;

namespace Commands.ActionPlugins;

public class DisplayMessage : IActionPlugin
{
    public string Name => "Display Message";

    public ActionType Type => ActionType.UI;

    public async Task ExecuteAsync(Core.Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
    {
        var textBlock = new TextBlock
        {
            Text = action.Parameters["Message"],
            Height = 32
        };
        var dialog = new ContentDialog
        {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Content = textBlock,
            Title = action.Parameters["Title"],
            IsSecondaryButtonEnabled = false,
            PrimaryButtonText = "Ok",
        };

        outputDataReceivedHandler($"Show message dialog {textBlock.Text}");

        await dialog.ShowAsync();
    }
    public Dictionary<string, string> GetDefaultParameters()
    {
        return new()
        {
            { "Title", string.Empty },
            { "Message", string.Empty }
        };
    }

    public IEnumerable<string> GetVariableNames()
    {
        return Array.Empty<string>();
    }

    public bool IsAvailable() => true;
}
