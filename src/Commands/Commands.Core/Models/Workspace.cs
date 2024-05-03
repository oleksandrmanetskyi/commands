namespace Commands.Core.Models;

public class Workspace
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public List<Command> Commands { get; set; } = new();
}
