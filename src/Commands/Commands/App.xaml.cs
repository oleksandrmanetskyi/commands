﻿using Commands.Activation;
using Commands.Contracts.Services;
using Commands.Core;
using Commands.Core.Contracts.Services;
using Commands.Core.Services;
using Commands.Helpers;
using Commands.Notifications;
using Commands.Services;
using Commands.ViewModels;
using Commands.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Windows.Globalization;

namespace Commands;

public partial class App : Application
{
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

            // Services
            services.AddSingleton<IAppNotificationService, AppNotificationService>();
            services.AddSingleton<ILocalStorageService, LocalStorageService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            // services.AddSingleton<ICommandsDataService, CommandsDataService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ActionsRegistry>();
            services.AddSingleton<WorkspacesDataService>();

            // Plugins Initializer
            services.AddSingleton<CoreActionPluginsInitializer>();
            services.AddSingleton<UiActionPluginsInitializer>();

            // Command Executor
            services.AddScoped<CommandExecutor>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<CommandDetailViewModel>();
            services.AddTransient<CommandsDetailPage>();
            services.AddTransient<CommandsViewModel>();
            services.AddTransient<CommandsPage>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();
        }).
        Build();

        GetService<IAppNotificationService>().Initialize();
        GetService<CoreActionPluginsInitializer>().Initialize();
        GetService<UiActionPluginsInitializer>().Initialize();

        UnhandledException += App_UnhandledException;

        ApplicationLanguages.PrimaryLanguageOverride = "en-US";
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        GetService<IAppNotificationService>().Show(string.Format("AppNotificationSamplePayload".GetLocalized(), AppContext.BaseDirectory));

        await GetService<IActivationService>().ActivateAsync(args);
    }
}
