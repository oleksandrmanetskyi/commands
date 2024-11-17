using Commands.Behaviors;
using Commands.Core.ActionPlugins;
using Commands.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace Commands.Views;

public sealed partial class CommandsDetailPage : Page
{
    public CommandDetailViewModel ViewModel
    {
        get;
    }

    public CommandsDetailPage()
    {
        ViewModel = App.GetService<CommandDetailViewModel>();
        InitializeComponent();

        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }

    private void AvaliableActionsListView_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is IActionPlugin item)
        {
            ViewModel.CreateNewAction(item.Name);
        }
    }
}
