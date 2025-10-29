using System.Diagnostics;
using System.Text;
using Commands.Core.Models;

namespace Commands.Core.ActionPlugins;

public class PowerShellActionPlugin : IActionPlugin
{
    public string Name => "PowerShell";

    public ActionType Type => ActionType.CommandLine;

    private readonly string commandLinePath = "pwsh.exe";

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
        outputDataReceivedHandler($"[{DateTime.Now}] - Executing {Name} action");
        var arguments = new StringBuilder();
        arguments.Append(" -Command \"");
        arguments.Append(action.Parameters["Script"] + '\"');
        
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

        // Read both streams concurrently to avoid potential deadlock
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(outputTask, errorTask);

        var output = await outputTask;
        var error = await errorTask;

        outputDataReceivedHandler(output);

        await process.WaitForExitAsync();

        context.Variables.Add(action.VariableNames[0], new VariableInfo { Type = typeof(string), Value = output });
        context.Variables.Add(action.VariableNames[1], new VariableInfo { Type = typeof(string), Value = error });
    }

    public IEnumerable<string> GetVariableNames()
    {
        return new List<string>() { "PowerShellOutput", "PowerShellError" };
    }
}
