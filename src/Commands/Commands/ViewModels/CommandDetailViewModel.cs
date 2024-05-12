using System.Windows.Input;
using Commands.ActionPlugins;
using Commands.Contracts.ViewModels;
using Commands.Core;
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
    private readonly ActionsRegistry actionsService;
    private readonly CommandExecutor commandExecutor;

    [ObservableProperty]
    private Command? command;

    public CommandDetailViewModel(
        WorkspacesDataService workspacesDataService, ActionsRegistry actionsService, CommandExecutor commandExecutor)
    {
        this.workspacesDataService = workspacesDataService;
        this.actionsService = actionsService;
        this.commandExecutor = commandExecutor;

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
        // TODO: Mode to action plugin definition
        var layout = actionName == new TextInput().Name ? Layouts.TextInput : Layouts.CommandLine;

        var actionPlugin = actionsService.GetActionPluginByName(actionName) 
            ?? throw new InvalidOperationException($"Action plugin {actionName} not found");
        
        Command?.Actions.Add(new Core.Models.Action()
        {
            PluginName = actionName,
            Parameters = actionPlugin.GetDefaultParameters(),
            Layout = layout,
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
        await commandExecutor.ExecuteAsync(command);
        RunButtonEnabled = true;
    }

    private void StarCommand()
    {
        if (Command != null)
        {
            Command.Starred = !Command.Starred;
        }
        
    }
}
