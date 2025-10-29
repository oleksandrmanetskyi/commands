using Commands.Core.ActionPlugins;

namespace Commands.Core.Services;

public class ActionsService
{
    public readonly List<IActionPlugin> plugins = new();
    private readonly Dictionary<string, IActionPlugin> pluginsByName = new();

    public ActionsService()
    {
        
    }

    public void RegisterActionIfAvailable<TActionPlugin>()
        where TActionPlugin : IActionPlugin
    {
        var plugin = Activator.CreateInstance<TActionPlugin>();
        if (plugin.IsAvailable())
        {
            plugins.Add(plugin);
            pluginsByName[plugin.Name] = plugin;
        }
    }

    public IEnumerable<IActionPlugin> GetActionPlugins()
    {
        return plugins;
    }

    public IActionPlugin GetActionPluginByName(string name)
    {
        // Use dictionary lookup for O(1) performance instead of linear search
        return pluginsByName.TryGetValue(name, out var plugin) ? plugin : null;
    }
}
