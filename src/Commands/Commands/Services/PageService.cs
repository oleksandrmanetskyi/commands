using Commands.Contracts.Services;
using Commands.ViewModels;
using Commands.Views;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.UI.Xaml.Controls;

namespace Commands.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> pages = new();

    public PageService()
    {
        Configure<HomeViewModel, MainPage>();
        Configure<CommandsViewModel, CommandsPage>();
        Configure<CommandDetailViewModel, CommandsDetailPage>();
        Configure<SettingsViewModel, SettingsPage>();
    }

    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (pages)
        {
            if (!pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    private void Configure<VM, V>()
        where VM : ObservableObject
        where V : Page
    {
        lock (pages)
        {
            var key = typeof(VM).FullName!;
            if (pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(V);
            if (pages.Any(p => p.Value == type))
            {
                throw new ArgumentException($"This type is already configured with key {pages.First(p => p.Value == type).Key}");
            }

            pages.Add(key, type);
        }
    }
}
