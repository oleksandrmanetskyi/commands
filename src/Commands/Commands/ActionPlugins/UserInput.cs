using Commands.Core.ActionPlugins;
using Commands.Core.Models;
using Commands.Helpers;
using Microsoft.UI.Xaml.Controls;

namespace Commands.ActionPlugins;

public class UserInput : IActionPlugin
{
    public string Name => "User Input";

    public ActionType Type => ActionType.UI;

    public Dictionary<string, string> GetDefaultParameters() => new()
    {
        { "Placeholder", string.Empty },
        { "DefaultValue", string.Empty },
    };

    public async Task ExecuteAsync(Core.Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
    {
        outputDataReceivedHandler($"[{DateTime.Now}] - Executing {Name} action");
        var inputTextBox = new TextBox
        {
            AcceptsReturn = false,
            Height = 32,
            PlaceholderText = action.Parameters["Placeholder"],
            Text = action.Parameters["DefaultValue"]
        };
        var dialog = new ContentDialog
        {
            XamlRoot = App.MainWindow.Content.XamlRoot,
            Content = inputTextBox,
            Title = "InputStringValue".GetLocalized(),
            IsSecondaryButtonEnabled = true,
            PrimaryButtonText = "Ok".GetLocalized(),
            SecondaryButtonText = "Cancel".GetLocalized()
        };

        outputDataReceivedHandler($"Show dialog {inputTextBox.PlaceholderText}");
        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            context.Variables[action.VariableNames[0]] = new VariableInfo
            {
                Value = inputTextBox.Text,
                Type = typeof(string)
            };
        }
    }
    public bool IsAvailable() => true;
    public IEnumerable<string> GetVariableNames() => new List<string> { "UserInput" };
}
