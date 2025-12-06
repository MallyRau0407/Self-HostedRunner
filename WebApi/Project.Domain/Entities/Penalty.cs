namespace Project.Domain.Entities;

public class Penalty
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid UserId { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? EndsAt { get; private set; }

    public bool IsActive => EndsAt == null || EndsAt > DateTime.UtcNow;

    private Penalty() { }

    public Penalty(Guid userId, string reason, DateTime? endsAt)
    {
        UserId = userId;
        Reason = reason;
        EndsAt = endsAt;
    }
}
