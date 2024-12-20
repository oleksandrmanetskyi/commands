﻿using Commands.Core.ActionPlugins;

namespace Commands.Core.Services;

public class ActionsRegistry
{
    public readonly List<IActionPlugin> plugins = new();

    public ActionsRegistry()
    {
        
    }

    public void RegisterActionIfAvailable<TActionPlugin>()
        where TActionPlugin : IActionPlugin
    {
        var plugin = Activator.CreateInstance<TActionPlugin>();
        if (plugin.IsAvailable())
        {
            plugins.Add(plugin);
        }
    }

    public IEnumerable<IActionPlugin> GetActionPlugins()
    {
        return plugins;
    }

    public IActionPlugin GetActionPluginByName(string name)
    {
        return plugins.First(p => p.Name == name);
    }
}
