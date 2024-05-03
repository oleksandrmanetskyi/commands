using System.Windows.Input;

using Commands.Contracts.Services;
using Commands.Helpers;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace Commands.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService themeSelectorService;
    [ObservableProperty]
    private ElementTheme elementTheme;
    [ObservableProperty]
    private string versionDescription;

    public ICommand SwitchThemeCommand { get; }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        this.themeSelectorService = themeSelectorService;
        elementTheme = this.themeSelectorService.Theme;
        versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await this.themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        var packageVersion = Package.Current.Id.Version;

        var version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
