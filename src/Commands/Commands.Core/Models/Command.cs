using System.Collections.ObjectModel;

namespace Commands.Core.Models;

public class Command
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public bool Starred { get; set; } = false;
    public ObservableCollection<Action> Actions { get; set; } = new();
}
