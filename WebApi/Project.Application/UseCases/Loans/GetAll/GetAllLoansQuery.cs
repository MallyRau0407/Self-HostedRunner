using MediatR;
using Project.Application.Dtos;

namespace Project.Application.UseCases.Loans.GetAll
{
    public record GetAllLoansQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<LoanDto>>;
}
