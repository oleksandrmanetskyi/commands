using Commands.Core.ActionPlugins;
using Commands.Core.Models;
using Commands.Core.Services;

namespace Commands.Tests.MSTest.Services;

[TestClass]
public class ActionsRegistryTests
{
    private class TestActionPlugin : IActionPlugin
    {
        public string Name => "TestAction";
        public ActionType Type => ActionType.CommandLine;
        
        public bool IsAvailable() => true;
        
        public Dictionary<string, string> GetDefaultParameters() => new();
        
        public Task ExecuteAsync(Commands.Core.Models.Action action, CommandExecutorContext context, Action<string> outputDataReceivedHandler)
        {
            return Task.CompletedTask;
        }
        
        public IEnumerable<string> GetVariableNames() => Array.Empty<string>();
    }

    [TestMethod]
    public void GetActionPluginByName_ReturnsCorrectPlugin()
    {
        // Arrange
        var registry = new ActionsRegistry();
        registry.RegisterActionIfAvailable<TestActionPlugin>();

        // Act
        var plugin = registry.GetActionPluginByName("TestAction");

        // Assert
        Assert.IsNotNull(plugin);
        Assert.AreEqual("TestAction", plugin.Name);
    }

    [TestMethod]
    public void GetActionPluginByName_ReturnsNull_WhenPluginNotFound()
    {
        // Arrange
        var registry = new ActionsRegistry();

        // Act
        var plugin = registry.GetActionPluginByName("NonExistent");

        // Assert
        Assert.IsNull(plugin);
    }

    [TestMethod]
    public void RegisterActionIfAvailable_AddsPluginToList()
    {
        // Arrange
        var registry = new ActionsRegistry();

        // Act
        registry.RegisterActionIfAvailable<TestActionPlugin>();

        // Assert
        var plugins = registry.GetActionPlugins().ToList();
        Assert.AreEqual(1, plugins.Count);
        Assert.AreEqual("TestAction", plugins[0].Name);
    }
}
