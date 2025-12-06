using MediatR;
using Project.Application.Dtos;
using Project.Domain.IRepositories;
using System.Linq;

namespace Project.Application.UseCases.Loans.GetAll
{
    public class GetAllLoansHandler : IRequestHandler<GetAllLoansQuery, IEnumerable<LoanDto>>
    {
        private readonly ILoanRepository _loanRepo;
        public GetAllLoansHandler(ILoanRepository loanRepo) => _loanRepo = loanRepo;

        public async Task<IEnumerable<LoanDto>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepo.GetAllAsync(request.Page, request.PageSize);
            return loans.Select(l => new LoanDto(l.Id, l.UserId, l.ResourceId, l.StartDate, l.DueDate, l.ReturnDate, l.Status.ToString()));
        }
    }
}
