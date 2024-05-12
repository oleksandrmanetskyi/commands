using Commands.Core.ActionPlugins;
using Commands.Core.Models;
using Microsoft.UI.Xaml.Controls;

namespace Commands.ActionPlugins;

public class TextInput : IActionPlugin
{
    public string Name => "Text Input";

    public ActionType Type => ActionType.UI;

    public Dictionary<string, string> GetDefaultParameters() => new()
    {
        { "Placeholder", string.Empty },
        { "DefaultValue", string.Empty },
        { "VariableName", string.Empty }
    };

    public async Task ExecuteAsync(Core.Models.Action action, CommandExecutorContext context)
    {
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
            Title = "Input string value",
            IsSecondaryButtonEnabled = true,
            PrimaryButtonText = "Ok",
            SecondaryButtonText = "Cancel"
        };

        if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        {
            context.Variables[action.Parameters["VariableName"]] = new VariableInfo
            {
                Value = inputTextBox.Text,
                Type = typeof(string)
            };
        }
    }
    public bool IsAvailable() => true;
}
