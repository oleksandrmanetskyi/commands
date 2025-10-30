using Commands.Contracts.Services;
using Commands.Core.Models;

namespace Commands.Core.Services;

public class WorkspacesDataService
{
    private readonly List<Workspace> allWorkspaces = new();
    private readonly ILocalStorageService localStorageService;
    private readonly string workspacesFolder = "Workspaces";

    public WorkspacesDataService(ILocalStorageService localStorageService) 
    {
        this.localStorageService = localStorageService;
        // Use GetAwaiter().GetResult() instead of Task.Run().Wait() to avoid thread pool issues
        Load().GetAwaiter().GetResult();
    }

    public IEnumerable<Workspace> GetWorkpaces()
    {
        return allWorkspaces;
    }

    public Guid CreateNewWorkspace(string name)
    {
        var workspace = new Workspace
        {
            Name = name
        };
        allWorkspaces.Add(workspace);

        return workspace.Id;
    }

    public IEnumerable<Command> GetWorkspaceCommands(Guid workspaceId)
    {
        var workspace = allWorkspaces.Find(x => x.Id == workspaceId);
        if (workspace != null)
        {
            return workspace.Commands;
        }

        return Array.Empty<Command>();
    }

    public void AddCommandToWorkspace(Guid workspaceId, Command command)
    {
        var workspace = allWorkspaces.Find(x => x.Id == workspaceId);
        workspace?.Commands.Add(command);
    }

    public void RemoveCommand(Guid commandId)
    {
        foreach (var workspace in allWorkspaces)
        {
            var command = workspace.Commands.FirstOrDefault(x => x.Id == commandId);
            if (command != null)
            {
                workspace.Commands.Remove(command);
                return;
            }
        }
    }

    public Command GetCommand(Guid commandId)
    {
        foreach (var workspace in allWorkspaces)
        {
            var command = workspace.Commands.FirstOrDefault(x => x.Id == commandId);
            if (command != null)
            {
                return command;
            }
        }

        return null;
    }

    private async Task Load()
    {
        try
        {
            var workspaces = localStorageService.GetSavedDataNames(workspacesFolder);

            await foreach (var workspace in workspaces)
            {
                var workspaceData = await localStorageService.ReadDataAsync<Workspace>(workspace, workspacesFolder);
                if (workspaceData != null)
                {
                    allWorkspaces.Add(workspaceData);
                }
            }
        }
        catch {
            
        }
    }

    public async Task SaveWorkspacesToApplicationData()
    {
        foreach (var workspace in allWorkspaces)
        {
            await localStorageService.SaveDataAsync(workspace.Id.ToString(), workspace, workspacesFolder);
        }
    }

    public IEnumerable<Command> GetStarredCommands()
    {
        return allWorkspaces.SelectMany(x => x.Commands).Where(x => x.Starred);
    }
}
