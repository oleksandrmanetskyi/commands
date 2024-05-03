using Commands.Core.ActionPlugins;
using Commands.Core.Services;

namespace Commands.Core;

public class ActionPluginsInitializer
{
    private readonly ActionsService service;

    public ActionPluginsInitializer(ActionsService service)
    {
        this.service = service;
    }

    public void Initialize()
    {
        service.RegisterActionIfAvailable<CommandPromptActionPlugin>();
        service.RegisterActionIfAvailable<PowerShellActionPlugin>();
    }
}
