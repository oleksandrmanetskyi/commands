using Commands.ActionPlugins;
using Commands.Core.Services;

namespace Commands;

public class UiActionPluginsInitializer
{
    private readonly ActionsRegistry service;

    public UiActionPluginsInitializer(ActionsRegistry service)
    {
        this.service = service;
    }

    public void Initialize()
    {
        service.RegisterActionIfAvailable<UserInput>();
    }
}
