using MediatR;

namespace Project.Application.UseCases.Loans.Create
{
    public record CreateLoanCommand(Guid UserId, Guid ResourceId, DateTime StartDate, DateTime DueDate) : IRequest<Guid>;
}
