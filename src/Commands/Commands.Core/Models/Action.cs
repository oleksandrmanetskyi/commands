using Commands.Core.Services;

namespace Commands.Core.Models;

public class Action
{
    public string PluginName { get; set; }
    public Dictionary<string, string> Parameters { get; set; } = new();
    public Layouts Layout { get; set; }
}
