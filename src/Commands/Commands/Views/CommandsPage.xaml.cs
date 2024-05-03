using Commands.Behaviors;
using Commands.Core.Models;
using Commands.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace Commands.Views;

public sealed partial class CommandsPage : Page
{
    public CommandsViewModel ViewModel
    {
        get;
    }

    public CommandsPage()
    {
        ViewModel = App.GetService<CommandsViewModel>();
        InitializeComponent();

        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.NavigateToCommand(e.ClickedItem as Command);
    }
}
