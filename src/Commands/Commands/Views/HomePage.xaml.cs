using Commands.Core.Models;
using Commands.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Commands.Views;

public sealed partial class MainPage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }

    private void GridView_ItemClick(object sender, ItemClickEventArgs e)
    {
        ViewModel.NavigateToCommand(e.ClickedItem as Command);
    }
}
