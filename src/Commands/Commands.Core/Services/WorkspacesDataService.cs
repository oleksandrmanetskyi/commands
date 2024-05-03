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
        Task.Run(Load).Wait();
    }

    public IEnumerable<Workspace> GetWorkpaces()
    {
        return allWorkspaces;
    }

    public Guid CreateNewWorkspace(string name)
    {
        allWorkspaces.Add(new()
        {
            Name = name
        });

        return allWorkspaces.Last().Id;
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
            var command = GetCommandFromWorkspace(workspace, commandId);
            if (command != null)
            {
                return;
            }
        }
    }

    public Command GetCommand(Guid commandId)
    {
        foreach (var workspace in allWorkspaces)
        {
            var command = GetCommandFromWorkspace(workspace, commandId);
            if (command != null)
            {
                return command;
            }
        }

        return null;
    }

    private static Command GetCommandFromWorkspace(Workspace workspace, Guid commandId)
    {
        return workspace.Commands.FirstOrDefault(x => x.Id == commandId);
    }

    private async Task Load()
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
