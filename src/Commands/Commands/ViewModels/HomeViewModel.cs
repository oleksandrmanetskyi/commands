using System.Collections.ObjectModel;

using Commands.Contracts.Services;
using Commands.Contracts.ViewModels;
using Commands.Core.Contracts.Services;
using Commands.Core.Models;
using Commands.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Commands.ViewModels;

public class HomeViewModel : ObservableRecipient, INavigationAware
{
    public ObservableCollection<Command> StarredCommands { get; } = new();

    private readonly INavigationService navigationService;
    private readonly WorkspacesDataService workspacesDataService;

    public HomeViewModel(INavigationService navigationService, WorkspacesDataService workspacesDataService)
    {
        this.navigationService = navigationService;
        this.workspacesDataService = workspacesDataService;
    }

    public void OnNavigatedTo(object parameter)
    {
        var commands = workspacesDataService.GetStarredCommands();
        foreach (var item in commands)
        {
            StarredCommands.Add(item);
        }
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
}
