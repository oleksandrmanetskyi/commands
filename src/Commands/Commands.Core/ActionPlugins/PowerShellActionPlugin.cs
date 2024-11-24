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
            { "KeepShowWindow", "false" }
        };
    }

    public async Task ExecuteAsync(Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
    {
        var keepShowWindow = action.Parameters["KeepShowWindow"].ToLower() == "true";
        var arguments = new StringBuilder();
        if (keepShowWindow)
        {
            arguments.Append("-NoExit");
        }
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

        using var process = new Process
        {
            StartInfo = start,
            EnableRaisingEvents = true
        };

        process.OutputDataReceived += (sender, args) =>
        {
            outputDataReceivedHandler(args.Data);
        };
        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                outputDataReceivedHandler(args.Data);
            }
        };

        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        outputDataReceivedHandler(output);
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        context.Variables.Add(action.VariableNames[0], new VariableInfo { Type = typeof(string), Value = output });
        context.Variables.Add(action.VariableNames[1], new VariableInfo { Type = typeof(string), Value = error });
    }

    public IEnumerable<string> GetVariableNames()
    {
        return new List<string>() { "PowerShellOutput", "PowerShellError" };
    }
}
