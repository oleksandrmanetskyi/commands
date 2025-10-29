using Commands.Core.Helpers;
using Newtonsoft.Json;

namespace Commands.Tests.MSTest.Helpers;

[TestClass]
public class JsonHelperTests
{
    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    [TestMethod]
    public async Task ToObjectAsync_DeserializesJsonCorrectly()
    {
        // Arrange
        var json = "{\"Name\":\"Test\",\"Value\":42}";

        // Act
        var result = await Json.ToObjectAsync<TestObject>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual(42, result.Value);
    }

    [TestMethod]
    public async Task StringifyAsync_SerializesObjectCorrectly()
    {
        // Arrange
        var obj = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await Json.StringifyAsync(obj);

        // Assert
        Assert.IsNotNull(result);
        var deserialized = JsonConvert.DeserializeObject<TestObject>(result);
        Assert.IsNotNull(deserialized);
        Assert.AreEqual("Test", deserialized.Name);
        Assert.AreEqual(42, deserialized.Value);
    }

    [TestMethod]
    public async Task ToObjectAsync_HandlesNullJson()
    {
        // Act
        var result = await Json.ToObjectAsync<TestObject>(null);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task StringifyAsync_HandlesNullObject()
    {
        // Act
        var result = await Json.StringifyAsync(null);

        // Assert
        Assert.AreEqual("null", result);
    }
}
