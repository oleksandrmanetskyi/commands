using System.ComponentModel;
using System.Windows.Input;
using Commands.Contracts.ViewModels;
using Commands.Core.ActionPlugins;
using Commands.Core.Models;
using Commands.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;

namespace Commands.ViewModels;

public partial class CommandDetailViewModel : ObservableRecipient, INavigationAware
{
    public ICommand RunButtonClickCommand { get; }
    public ICommand StarCommandButtonClickCommand { get; }
    public bool RunButtonEnabled { get; set; }

    private readonly WorkspacesDataService workspacesDataService;
    private readonly ActionsService actionsService;

    [ObservableProperty]
    private Command? command;

    public CommandDetailViewModel(WorkspacesDataService workspacesDataService, ActionsService actionsService)
    {
        this.workspacesDataService = workspacesDataService;
        this.actionsService = actionsService;
        RunButtonEnabled = true;

        RunButtonClickCommand = new AsyncRelayCommand(ExecuteAsync);
        StarCommandButtonClickCommand = new RelayCommand(StarCommand);
    }

    public void OnNavigatedTo(object parameter)
    {
        if (parameter is Guid commandId)
        {
            Command = workspacesDataService.GetCommand(commandId);
        }
    }

    public void OnNavigatedFrom()
    {

    }

    public void CreateNewAction(string actionName)
    {
        Command?.Actions.Add(new Core.Models.Action()
        {
            PluginName = actionName,
            Parameters = new()
            {
                { "Script", "" },
                { "KeepShowWindow", "false" }
            }
        });
    }

    public IEnumerable<IActionPlugin> GetActionPlugins()
    {
        return actionsService.GetActionPlugins();
    }
    public Visibility CommandStarredVisibility(bool starredOption)
    {
        if ((starredOption && Command!.Starred) || (!starredOption && !Command!.Starred))
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
        }
    }

    private async Task ExecuteAsync()
    {
        if (command == null)
        {
            return;
        }

        RunButtonEnabled = false;
        foreach (var action in command.Actions)
        {
            var handler = actionsService.GetActionPluginByName(action.PluginName);
            if (handler != null)
            {
                await handler.ExecuteAsync(action);
            }
        }
        RunButtonEnabled = true;
    }

    private void StarCommand()
    {
        Command!.Starred = !Command.Starred;
    }
}
