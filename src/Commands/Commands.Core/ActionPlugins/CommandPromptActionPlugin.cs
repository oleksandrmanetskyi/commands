using System.Diagnostics;
using System.Text;
using Commands.Core.Models;

namespace Commands.Core.ActionPlugins;

public class CommandPromptActionPlugin : IActionPlugin
{
    public string Name => "Command Prompt";

    public ActionType Type => ActionType.CommandLine;

    private readonly string commandLinePath = "cmd.exe";

    public bool IsAvailable()
    {
        return true;
    }

    public Dictionary<string, string> GetDefaultParameters()
    {
        return new()
        {
            { "Script", string.Empty },
            { "KeepShowWindow", "false" }
        };
    }

    public async Task ExecuteAsync(Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
    {
        var keepShowWindow = action.Parameters["KeepShowWindow"].ToLower() == "true";
        var arguments = new StringBuilder();
        arguments.Append(keepShowWindow ? "/K " : "/C " );
        arguments.Append(action.Parameters["Script"]);

        var start = new ProcessStartInfo
        {
            FileName = commandLinePath,
            Arguments = arguments.ToString(),
            UseShellExecute = true
        };

        using var process = Process.Start(start);

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        context.Variables.Add(action.VariableNames[0], new() { Type = typeof(string), Value = output });
        context.Variables.Add(action.VariableNames[1], new() { Type = typeof(string), Value = error });
    }

    public IEnumerable<string> GetVariableNames() => new List<string>() { "CommandPromptOutput", "CommandPromptError" };
}
