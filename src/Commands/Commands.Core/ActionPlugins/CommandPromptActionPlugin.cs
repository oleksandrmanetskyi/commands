using System.Diagnostics;
using System.Text;

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

    public async Task ExecuteAsync(Models.Action action)
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
        
        await process.WaitForExitAsync();
    }
}
