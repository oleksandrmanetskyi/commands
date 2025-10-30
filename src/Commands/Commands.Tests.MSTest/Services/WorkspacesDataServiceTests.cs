using Commands.Core.Models;
using Commands.Core.Services;
using Commands.Contracts.Services;

namespace Commands.Tests.MSTest.Services;

[TestClass]
public class WorkspacesDataServiceTests
{
    private class MockLocalStorageService : ILocalStorageService
    {
        private readonly List<Workspace> mockWorkspaces = new();

        public Task<T?> ReadSettingAsync<T>(string key)
        {
            return Task.FromResult<T?>(default);
        }

        public Task SaveSettingAsync<T>(string key, T value)
        {
            return Task.CompletedTask;
        }

        public Task<T?> ReadDataAsync<T>(string name, string? folderName = null)
        {
            if (typeof(T) == typeof(Workspace))
            {
                var workspace = mockWorkspaces.FirstOrDefault(w => w.Id.ToString() == name);
                return Task.FromResult<T?>((T?)(object?)workspace);
            }
            return Task.FromResult<T?>(default);
        }

        public Task SaveDataAsync<T>(string name, T data, string? folderName = null)
        {
            return Task.CompletedTask;
        }

        public async IAsyncEnumerable<string> GetSavedDataNames(string? folderName = null)
        {
            foreach (var workspace in mockWorkspaces)
            {
                yield return workspace.Id.ToString();
            }
            await Task.CompletedTask;
        }

        public void AddMockWorkspace(Workspace workspace)
        {
            mockWorkspaces.Add(workspace);
        }
    }

    [TestMethod]
    public void CreateNewWorkspace_AddsWorkspace()
    {
        // Arrange
        var mockStorage = new MockLocalStorageService();
        var service = new WorkspacesDataService(mockStorage);

        // Act
        var id = service.CreateNewWorkspace("Test Workspace");

        // Assert
        var workspaces = service.GetWorkpaces().ToList();
        Assert.AreEqual(1, workspaces.Count);
        Assert.AreEqual("Test Workspace", workspaces[0].Name);
        Assert.AreEqual(id, workspaces[0].Id);
    }

    [TestMethod]
    public void RemoveCommand_RemovesCommandFromWorkspace()
    {
        // Arrange
        var mockStorage = new MockLocalStorageService();
        var service = new WorkspacesDataService(mockStorage);
        var workspaceId = service.CreateNewWorkspace("Test Workspace");
        var command = new Command { Name = "Test Command" };
        service.AddCommandToWorkspace(workspaceId, command);

        // Act
        service.RemoveCommand(command.Id);

        // Assert
        var commands = service.GetWorkspaceCommands(workspaceId).ToList();
        Assert.AreEqual(0, commands.Count);
    }

    [TestMethod]
    public void GetCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var mockStorage = new MockLocalStorageService();
        var service = new WorkspacesDataService(mockStorage);
        var workspaceId = service.CreateNewWorkspace("Test Workspace");
        var command = new Command { Name = "Test Command" };
        service.AddCommandToWorkspace(workspaceId, command);

        // Act
        var result = service.GetCommand(command.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(command.Id, result.Id);
        Assert.AreEqual("Test Command", result.Name);
    }

    [TestMethod]
    public void GetCommand_ReturnsNull_WhenCommandNotFound()
    {
        // Arrange
        var mockStorage = new MockLocalStorageService();
        var service = new WorkspacesDataService(mockStorage);

        // Act
        var result = service.GetCommand(Guid.NewGuid());

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetStarredCommands_ReturnsOnlyStarredCommands()
    {
        // Arrange
        var mockStorage = new MockLocalStorageService();
        var service = new WorkspacesDataService(mockStorage);
        var workspaceId = service.CreateNewWorkspace("Test Workspace");
        
        var command1 = new Command { Name = "Command 1", Starred = true };
        var command2 = new Command { Name = "Command 2", Starred = false };
        var command3 = new Command { Name = "Command 3", Starred = true };
        
        service.AddCommandToWorkspace(workspaceId, command1);
        service.AddCommandToWorkspace(workspaceId, command2);
        service.AddCommandToWorkspace(workspaceId, command3);

        // Act
        var starred = service.GetStarredCommands().ToList();

        // Assert
        Assert.AreEqual(2, starred.Count);
        Assert.IsTrue(starred.All(c => c.Starred));
    }
}
