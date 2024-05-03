using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        foreach (var action in command.Actions)
        {
            var handler = actionsService.GetActionPluginByName(action.PluginName);
            if (handler != null)
            {
                await handler.ExecuteAsync(action);
            }
        }
    }
}
