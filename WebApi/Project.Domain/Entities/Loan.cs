using Project.Domain.Enums;

namespace Project.Domain.Entities;

public class Loan
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid UserId { get; private set; }
    public Guid ResourceId { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public LoanStatus Status { get; private set; }

    private Loan() { }

    public Loan(Guid userId, Guid resourceId, DateTime startDate, DateTime dueDate)
    {
        UserId = userId;
        ResourceId = resourceId;
        StartDate = startDate;
        DueDate = dueDate;
        Status = LoanStatus.Active;
    }

    public void Return(DateTime returnDate)
    {
        ReturnDate = returnDate;
        Status = LoanStatus.Returned;
    }

    public bool IsOverdue(DateTime currentDate)
    {
        return currentDate > DueDate && Status == LoanStatus.Active;
    }
}
