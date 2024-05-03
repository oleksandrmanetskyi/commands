using Commands.Core.Models;

namespace Commands.Core.Contracts.Services;

public interface ICommandsDataService
{
    IEnumerable<Command> GetAllComands();
    Command GetCommandById(Guid id);
    void AddCommand(Command command); 
}
