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

    private void TextBlock_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            var name = textBlock.Text;
            var inputTextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
                Text = name
            };
            var dialog = new ContentDialog
            {
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Content = inputTextBox,
                Title = "Input new variable name",
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            if (dialog.ShowAsync().GetResults() == ContentDialogResult.Primary)
            {
                textBlock.Text = inputTextBox.Text;
            }
        }
        
    }
}
