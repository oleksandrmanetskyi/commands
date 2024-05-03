using Microsoft.UI.Xaml.Controls;

namespace Commands.Controls;

public sealed partial class CreateWorkspaceDialogContent : Page
{
    public string WorkspaceName { get; set; }

    public CreateWorkspaceDialogContent()
    {
        this.InitializeComponent();
        WorkspaceName = string.Empty;
    }
}
