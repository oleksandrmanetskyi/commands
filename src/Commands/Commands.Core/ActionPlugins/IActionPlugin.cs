namespace Commands.Core.ActionPlugins;

public enum ActionType
{
    CommandLine
}

public interface IActionPlugin
{
    string Name { get; }
    ActionType Type { get; }
    bool IsAvailable();
    Task ExecuteAsync(Models.Action action);
}
