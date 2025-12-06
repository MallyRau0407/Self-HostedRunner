using Project.Domain.Enums;

namespace Project.Domain.Entities;

public class Resource
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = null!;
    public ResourceType Type { get; private set; }
    public bool IsAvailable { get; private set; } = true;

    private Resource() { }

    public Resource(string title, ResourceType type)
    {
        Title = title;
        Type = type;
    }

    public void MarkAsBorrowed()
    {
        IsAvailable = false;
    }

    public void MarkAsReturned()
    {
        IsAvailable = true;
    }
}
