# GitHub Copilot Instructions for Commands

## Project Overview

Commands is a Windows App SDK (WinUI 3) application designed to store and execute commands. It provides a user-friendly interface for managing command workspaces and executing various types of actions through a plugin-based architecture.

## Technology Stack

- **Framework**: .NET 7.0
- **UI Framework**: WinUI 3 (Windows App SDK 1.2)
- **Language**: C# with nullable reference types enabled
- **Architecture Pattern**: MVVM (Model-View-ViewModel)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Testing Framework**: MSTest
- **Package Manager**: NuGet

## Project Structure

The solution consists of three main projects:

### 1. Commands (Main Application)
- **Location**: `src/Commands/Commands/`
- **Type**: WinUI 3 Desktop Application
- **Platform Targets**: x86, x64, arm64
- **Purpose**: Main UI application with views, view models, and UI-specific services

Key directories:
- `Views/` - XAML pages (HomePage, CommandsPage, CommandDetailPage, SettingsPage, ShellPage)
- `ViewModels/` - View models following MVVM pattern
- `Services/` - Application-level services
- `Activation/` - Application activation handlers
- `Controls/` - Custom WinUI controls
- `ActionPlugins/` - UI-specific action plugins
- `Helpers/` - Utility classes

### 2. Commands.Core (Core Library)
- **Location**: `src/Commands/Commands.Core/`
- **Type**: .NET Standard Library
- **Purpose**: Core business logic, models, and reusable code
- **Target Framework**: net7.0 (platform-agnostic)

Key components:
- `Models/` - Domain models (Command, Action, Workspace, CommandExecutorContext, VariableInfo)
- `ActionPlugins/` - Plugin interface and core implementations (CommandPromptActionPlugin, PowerShellActionPlugin)
- `Services/` - Core services (ActionsRegistry, ActionsService, WorkspacesDataService, FileService)
- `CommandExecutor.cs` - Main command execution engine with variable interpolation
- `Contracts/` - Service interfaces

### 3. Commands.Tests.MSTest (Test Project)
- **Location**: `src/Commands/Commands.Tests.MSTest/`
- **Type**: MSTest test project
- **Purpose**: Unit and UI tests

## Architecture Patterns

### MVVM Pattern
- Views are XAML files located in `Views/`
- ViewModels are in `ViewModels/` and inherit from `ObservableObject` (CommunityToolkit.Mvvm)
- Models are in `Commands.Core/Models/`
- Use `[ObservableProperty]` attribute for bindable properties
- Use `[RelayCommand]` attribute for command methods

### Dependency Injection
- Services are registered in `App.xaml.cs` in the `ConfigureServices` method
- Use constructor injection for dependencies
- Service lifetimes:
  - Singleton: For services that maintain state across the app (e.g., ThemeSelectorService, LocalStorageService)
  - Transient: For stateless services or handlers
  - Scoped: Generally not used in desktop applications

### Plugin System
- Action plugins implement `IActionPlugin` interface
- Two types of action plugins:
  - `ActionType.CommandLine` - Execute command-line operations
  - `ActionType.UI` - Perform UI operations
- Plugins are registered in `ActionsRegistry`
- Core plugins initialized via `CoreActionPluginsInitializer`
- UI plugins initialized via `UiActionPluginsInitializer`

### Variable Interpolation
- The `CommandExecutor` supports variable interpolation using the pattern `{{variableName}}`
- Variables are stored in `CommandExecutorContext.Variables`
- Variables can be set by action plugins during execution
- Variables from previous actions can be used in subsequent actions

## Coding Conventions

### C# Style
- Use C# 11 features and file-scoped namespaces
- Enable nullable reference types (`<Nullable>enable</Nullable>`)
- Use implicit usings where enabled
- Follow Microsoft's C# naming conventions:
  - PascalCase for public members, types, and methods
  - camelCase for private fields (with `this.` prefix when accessing)
  - Prefix interfaces with `I`

### XAML Style
- Use `x:Bind` instead of `Binding` for better performance
- Follow WinUI 3 control patterns
- Use CommunityToolkit.WinUI controls when appropriate
- Organize resources in dedicated resource dictionaries

### File Organization
- One class per file
- File name should match the class name
- Group related files in appropriate folders
- Keep views and their code-behind together

## Key Dependencies

### NuGet Packages
- `Microsoft.WindowsAppSDK` (1.2.230118.102) - Windows App SDK
- `CommunityToolkit.Mvvm` (7.1.2) - MVVM helpers
- `CommunityToolkit.WinUI.UI.Controls` (7.1.2) - Additional WinUI controls
- `CommunityToolkit.WinUI.UI.Animations` (7.1.2) - Animation helpers
- `Microsoft.Extensions.Hosting` (6.0.1) - Dependency injection and hosting
- `Microsoft.Xaml.Behaviors.WinUI.Managed` (2.0.9) - XAML behaviors
- `WinUIEx` (2.1) - WinUI extensions
- `Newtonsoft.Json` (13.0.2) - JSON serialization (Core library)

## Building and Testing

### Build
```bash
# Build the solution
cd src/Commands
dotnet build Commands.sln

# Build specific project
dotnet build Commands/Commands.csproj
```

### Test
```bash
# Run all tests
cd src/Commands
dotnet test Commands.Tests.MSTest/Commands.Tests.MSTest.csproj

# Run tests with UI thread support using [UITestMethod] attribute
# UI tests must run on the WinUI UI thread
```

### Run
- Open solution in Visual Studio 2022
- Set `Commands` as startup project
- Build and run (F5)

## Common Patterns and Best Practices

### Adding a New View
1. Create XAML file in `Views/` folder
2. Create corresponding ViewModel in `ViewModels/` folder
3. Register ViewModel in DI container in `App.xaml.cs`
4. Register page with navigation service if needed
5. Add navigation item if it should appear in navigation menu

### Adding a New Service
1. Create interface in `Contracts/Services/`
2. Create implementation in `Services/`
3. Register in DI container in `App.xaml.cs` with appropriate lifetime
4. Inject via constructor where needed

### Adding a New Action Plugin
1. Implement `IActionPlugin` interface
2. Define `Name`, `Type`, and parameter structure
3. Implement `IsAvailable()` to check if plugin can run
4. Implement `GetDefaultParameters()` to provide parameter templates
5. Implement `GetVariableNames()` to expose variables the plugin sets
6. Implement `ExecuteAsync()` for the main execution logic
7. Register plugin in appropriate initializer:
   - Core plugins: `CoreActionPluginsInitializer`
   - UI plugins: `UiActionPluginsInitializer`

### Testing UI Components
- Use `[UITestMethod]` instead of `[TestMethod]` for tests that interact with UI
- Tests run on WinUI UI thread automatically
- Mock service interfaces for isolated testing
- Create mock implementations in `Mocks/` folder

### Working with Navigation
- Use `INavigationService` for page navigation
- Navigation parameters can be passed via `NavigateTo` method
- Handle navigation in ViewModel's `OnNavigatedTo` method if implementing `INavigationAware`

### Theme Support
- App supports Light, Dark, and System themes
- Theme is managed by `IThemeSelectorService`
- Theme setting is persisted in local storage
- Use theme-aware resources and styles

## Important Notes

- **Platform-specific**: This is a Windows-only application (WinUI 3)
- **Multi-targeting**: Main app targets Windows 10 version 19041 minimum
- **Packaging**: Uses MSIX packaging for deployment
- **File paths**: Use absolute paths when working with file services
- **Async/await**: Most service operations are async, always use async/await pattern
- **String interpolation**: Use variable pattern `{{variableName}}` in command parameters

## Development Workflow

1. Make changes to code
2. Build solution to verify no compilation errors
3. Run unit tests to ensure existing functionality works
4. Test manually in the running application
5. For UI changes, verify in both Light and Dark themes
6. Ensure no nullable reference warnings
7. Follow existing code patterns and conventions

## TODO Comments
- Check `View -> Task List` in Visual Studio for TODO comments
- These indicate areas that may need attention or completion

## Resources

- [WinUI 3 Documentation](https://docs.microsoft.com/windows/apps/winui/)
- [Windows App SDK](https://docs.microsoft.com/windows/apps/windows-app-sdk/)
- [MVVM Toolkit Documentation](https://docs.microsoft.com/dotnet/communitytoolkit/mvvm/)
- [Template Studio](https://github.com/microsoft/TemplateStudio) - Original template source
