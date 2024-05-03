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

    public async Task ExecuteAsync(Models.Action action, CommandExecutorContext _)
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
            UseShellExecute = true
        };

        using var process = Process.Start(start);
        
        await process.WaitForExitAsync();
    }
}
