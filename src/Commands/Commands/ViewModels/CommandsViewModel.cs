using System.Collections.ObjectModel;
using System.Windows.Input;

using Commands.Contracts.Services;
using Commands.Contracts.ViewModels;
using Commands.Core.Contracts.Services;
using Commands.Core.Models;
using Commands.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Commands.ViewModels;

public class CommandsViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<Command> Source { get; } = new();
    public ICommand CreateNewButtonClickCommand { get; }

    private readonly INavigationService navigationService;
    private readonly WorkspacesDataService workspacesDataService;
    private Guid workspaceId;

    public CommandsViewModel(INavigationService navigationService, WorkspacesDataService workspacesDataService)
    {
        this.navigationService = navigationService;
        this.workspacesDataService = workspacesDataService;

        workspaceId = Guid.Empty;
        CreateNewButtonClickCommand = new RelayCommand(NavigateToCreateNewCommand);
    }

    public void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        workspaceId = (Guid)parameter;
        var commands = workspacesDataService.GetWorkspaceCommands(workspaceId);
        foreach (var item in commands)
        {
            Source.Add(item);
        }

        workspacesDataService.SaveWorkspacesToApplicationData().Wait();
    }

    public void OnNavigatedFrom()
    {
    }

    public void NavigateToCommand(Command? command)
    {
        if (command != null)
        {
            navigationService.NavigateTo(typeof(CommandDetailViewModel).FullName!, command.Id);
        }
    }

    private void NavigateToCreateNewCommand()
    {
        var newCommand = new Command 
        {
            Name = "New command"
        };
        workspacesDataService.AddCommandToWorkspace(workspaceId, newCommand);
        NavigateToCommand(newCommand);
    }
}
