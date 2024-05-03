using Commands.Core.Contracts.Services;
using Commands.Core.Models;

namespace Commands.Core.Services;

public class CommandsDataService : ICommandsDataService
{
    private List<Command> allCommands;

    public CommandsDataService()
    {
    }

    private static IEnumerable<Command> LoadAllCommands()
    {
        // TODO: Get from JSON
        return new List<Command>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Command 1",
                Actions = new()
                {
                    new()
                    {
                        PluginName = "CommandPrompt",
                        Parameters = new()
                        {
                            { "Script", "dotnet build" },
                            { "KeepShowWindow", "true" }
                        }
                    },
                    new()
                    {
                        PluginName = "PowerShell",
                        Parameters = new()
                        {
                            { "Script", "dotnet restore" },
                            { "KeepShowWindow", "true" }
                        }
                    }
                }   
 
            }
        };
    }

    public IEnumerable<Command> GetAllComands()
    {
        allCommands ??= new List<Command>(LoadAllCommands());
        
        return allCommands;
    }

    public Command GetCommandById(Guid id)
    {
        return allCommands.First(c => c.Id == id);
    }

    public void AddCommand(Command command)
    {
        allCommands.Add(command);
    }
}
