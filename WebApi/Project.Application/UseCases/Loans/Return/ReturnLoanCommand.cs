using MediatR;

namespace Project.Application.UseCases.Loans.Return
{
    public record ReturnLoanCommand(Guid LoanId, DateTime ReturnDate)
        : IRequest<ReturnLoanResponse>;
}
