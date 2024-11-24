using System.Text;
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
using Windows.UI.Popups;

namespace Commands.ViewModels;

public partial class CommandDetailViewModel : ObservableRecipient, INavigationAware
{
    public ICommand RunButtonClickCommand { get; }
    public ICommand StarCommandButtonClickCommand { get; }
    public ICommand CloseOutputCommandButtonClickCommand { get; }

    private readonly WorkspacesDataService workspacesDataService;
    private readonly ActionsRegistry actionsService;
    private readonly CommandExecutor commandExecutor;

    private readonly List<string> variables;

    [ObservableProperty]
    private Command? command;
    [ObservableProperty]
    private bool commandIsRunning;
    [ObservableProperty]
    private string? commandOutput;
    [ObservableProperty]
    private bool commandOutputViewOpened;

    public CommandDetailViewModel(
        WorkspacesDataService workspacesDataService, ActionsRegistry actionsService, CommandExecutor commandExecutor)
    {
        this.workspacesDataService = workspacesDataService;
        this.actionsService = actionsService;
        this.commandExecutor = commandExecutor;

        variables = new List<string>();

        CommandIsRunning = false;
        CommandOutputViewOpened = false;

        RunButtonClickCommand = new AsyncRelayCommand(ExecuteAsync);
        StarCommandButtonClickCommand = new RelayCommand(StarCommand);
        CloseOutputCommandButtonClickCommand = new RelayCommand(CloseOutput);
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
        if (CommandIsRunning)
        {
            return;
        }
    }

    public void OnOutputDataReceived(string output)
    {
        CommandOutput += output + '\n';
    }

    public void CreateNewAction(string actionName)
    {
        // TODO: Mode to action plugin definition
        var layout = actionName == new UserInput().Name 
            ? Layouts.UserInput 
            : actionName == new DisplayMessage().Name 
            ? Layouts.DisplayMessage 
            : Layouts.CommandLine;

        var actionPlugin = actionsService.GetActionPluginByName(actionName) 
            ?? throw new InvalidOperationException($"Action plugin {actionName} not found");

        var variableNames = actionPlugin.GetVariableNames().Select(CreateVariable);

        Command?.Actions.Add(new Core.Models.Action()
        {
            PluginName = actionName,
            Parameters = actionPlugin.GetDefaultParameters(),
            Layout = layout,
            VariableNames = variableNames.ToList()
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

    public void RenameVariable(string oldVariableName, string newVariableName)
    {
        if (variables.Contains(oldVariableName))
        {
            variables.Remove(oldVariableName);
            variables.Add(newVariableName);
        }
    }

    private async Task ExecuteAsync()
    {
        if (command == null)
        {
            return;
        }

        CommandIsRunning = true;
        CommandOutputViewOpened = true;
        await commandExecutor.ExecuteAsync(command, OnOutputDataReceived);
        CommandIsRunning = false;
    }

    private void StarCommand()
    {
        if (Command != null)
        {
            Command.Starred = !Command.Starred;
        }
    }

    private void CloseOutput()
    {
        CommandOutputViewOpened = false;
        CommandOutput = string.Empty;
        // todo cancel
    }

    private string CreateVariable(string variableName)
    {
        var result = variableName;
        if (variables.Contains(variableName))
        {
            var i = 1;
            while (variables.Contains(result))
            {
                result = $"{variableName}{i}";
                i++;
            }
        }

        variables.Add(result);

        return result;
    }
}
