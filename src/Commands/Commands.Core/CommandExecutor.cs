using System.Text.RegularExpressions;
using Commands.Core.Models;
using Commands.Core.Services;

namespace Commands.Core;

public partial class CommandExecutor
{
    private readonly ActionsRegistry actionsService;

    public CommandExecutor(ActionsRegistry actionsService)
    {
        this.actionsService = actionsService;
    }
    public async Task ExecuteAsync(Command command, Action<string> outputDataReceivedHandler)
    {
        var context = new CommandExecutorContext();

        foreach (var action in command.Actions)
        {
            var handler = actionsService.GetActionPluginByName(action.PluginName);
            if (handler != null)
            {
                // TODO: Change name
                action.Parameters = InterpolateStrings(action.Parameters, context);
                await handler.ExecuteAsync(action, context, outputDataReceivedHandler);
            }
        }
    }

    private static Dictionary<string, string> InterpolateStrings(Dictionary<string, string> original, CommandExecutorContext context)
    {
        var interpolatedDictionary = new Dictionary<string, string>();
        var replacements = context.Variables;

        foreach (var pair in original)
        {
            var value = pair.Value;
            var matches = VariableRegex().Matches(value);
            foreach (Match match in matches)
            {
                var placeholder = match.Groups[1].Value;
                if (replacements.TryGetValue(placeholder, out var replacment))
                {
                    value = value.Replace(match.Value, (string)replacment.Value);
                }
            }
            interpolatedDictionary.Add(pair.Key, value);
        }

        return interpolatedDictionary;
    }

    [GeneratedRegex("\\{\\{(.*?)\\}\\}")]
    private static partial Regex VariableRegex();
}
