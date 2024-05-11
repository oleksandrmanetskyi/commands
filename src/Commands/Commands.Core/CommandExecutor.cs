using Commands.Core.Models;
using Commands.Core.Services;

namespace Commands.Core;

public class CommandExecutor
{
    private readonly ActionsRegistry actionsService;

    public CommandExecutor(ActionsRegistry actionsService)
    {
        this.actionsService = actionsService;
    }
    public async Task ExecuteAsync(Command command)
    {
        var context = new CommandExecutorContext();

        foreach (var action in command.Actions)
        {
            var handler = actionsService.GetActionPluginByName(action.PluginName);
            if (handler != null)
            {
                await handler.ExecuteAsync(action, context);
            }
        }
    }
}
