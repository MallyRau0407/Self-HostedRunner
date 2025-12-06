using MediatR;
using Project.Application.Dtos;

namespace Project.Application.UseCases.Loans.GetByUser
{
    public record GetLoansByUserQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<IEnumerable<LoanDto>>;
}
