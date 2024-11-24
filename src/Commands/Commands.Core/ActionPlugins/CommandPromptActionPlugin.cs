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
        };
    }

    public async Task ExecuteAsync(Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
    {
        var arguments = new StringBuilder();
        arguments.Append("/C ");
        arguments.Append(action.Parameters["Script"]);

        var start = new ProcessStartInfo
        {
            FileName = commandLinePath,
            Arguments = arguments.ToString(),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
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
