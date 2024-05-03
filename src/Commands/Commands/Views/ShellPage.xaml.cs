using Commands.Contracts.Services;
using Commands.Controls;
using Commands.Core.Services;
using Commands.Helpers;
using Commands.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Commands.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{
    public ShellViewModel ViewModel
    {
        get;
    }

    private readonly WorkspacesDataService workspacesDataService;
    private readonly INavigationService navigationService;

    public ShellPage(ShellViewModel viewModel, WorkspacesDataService workspacesDataService, INavigationService navigationService)
    {
        ViewModel = viewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        // TODO: Set the title bar icon by updating /Assets/WindowIcon.ico.
        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        AppTitleBarText.Text = "AppDisplayName".GetLocalized();

        this.navigationService = navigationService;

        this.workspacesDataService = workspacesDataService;
        LoadWorkspacesMenuItems();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
        
    }

    private void AutoSuggestBox_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        e.Handled = true;
    }

    private void LoadWorkspacesMenuItems()
    {
        foreach (var workspace in workspacesDataService.GetWorkpaces())
        {
            AddNewWorkspaceMenuItem(workspace.Name, workspace.Id);
        }

        // Create New Workspace button element
        AddNewWorkspaceMenuItem("CreateNewWorkspace".GetLocalized(), null, null, "\uE710");
    }

    private NavigationViewItem AddNewWorkspaceMenuItem(string name, object? id, int? index = null, string glyph = "\uE81E")
    {
        var workspaceElement = new NavigationViewItem
        {
            Content = name,
            Tag = id,
            Icon = new FontIcon
            {
                Glyph = glyph,
            }
        };
        workspaceElement.Tapped += WorkspaceElement_Tapped;

        if (index == null)
        {
            NavigationViewControl.MenuItems.Add(workspaceElement);
        }
        else
        {
            NavigationViewControl.MenuItems.Insert(index.Value, workspaceElement);
        }

        return workspaceElement;
    }

    private async void WorkspaceElement_Tapped(object sender, TappedRoutedEventArgs e)
    {
        var element = (NavigationViewItem)sender;
        var workspaceId = element.Tag;

        if (workspaceId == null)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Create a new workspace",
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                Content = new CreateWorkspaceDialogContent()
            };

            await dialog.ShowAsync();

            var name = ((CreateWorkspaceDialogContent)dialog.Content).WorkspaceName;
            workspaceId = workspacesDataService.CreateNewWorkspace(name);
            var newWorkspaceItem = AddNewWorkspaceMenuItem(name, workspaceId, NavigationViewControl.MenuItems.Count - 1);
            NavigationViewControl.SelectedItem = newWorkspaceItem;
        }
        
        if (workspaceId != null)
        {
            navigationService.NavigateTo(typeof(CommandsViewModel).FullName!, workspaceId);
        }
    }
}
