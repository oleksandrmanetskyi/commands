using Commands.Core.ActionPlugins;
using Commands.Core.Services;

namespace Commands.Core;

public class CoreActionPluginsInitializer
{
    private readonly ActionsRegistry service;

    public CoreActionPluginsInitializer(ActionsRegistry service)
    {
        this.service = service;
    }

    public void Initialize()
    {
        service.RegisterActionIfAvailable<CommandPromptActionPlugin>();
        service.RegisterActionIfAvailable<PowerShellActionPlugin>();
    }
}
