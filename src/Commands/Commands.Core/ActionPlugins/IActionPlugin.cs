using Commands.Core.Models;

namespace Commands.Core.ActionPlugins;

public enum ActionType
{
    CommandLine,
    UI
}

public interface IActionPlugin
{
    string Name { get; }
    ActionType Type { get; }
    bool IsAvailable();
    Dictionary<string, string> GetDefaultParameters();
    IEnumerable<string> GetVariableNames();
    Task ExecuteAsync(Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler);
}
