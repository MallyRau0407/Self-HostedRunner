using MediatR;
using Project.Application.Dtos;
using Project.Domain.IRepositories;
using System.Linq;

namespace Project.Application.UseCases.Loans.GetByUser
{
    public class GetLoansByUserHandler : IRequestHandler<GetLoansByUserQuery, IEnumerable<LoanDto>>
    {
        private readonly ILoanRepository _loanRepo;
        public GetLoansByUserHandler(ILoanRepository loanRepo) => _loanRepo = loanRepo;

        public async Task<IEnumerable<LoanDto>> Handle(GetLoansByUserQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepo.GetByUserIdAsync(request.UserId, request.Page, request.PageSize);
            return loans.Select(l => new LoanDto(l.Id, l.UserId, l.ResourceId, l.StartDate, l.DueDate, l.ReturnDate, l.Status.ToString()));
        }
    }
}
