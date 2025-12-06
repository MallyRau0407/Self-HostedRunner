using MediatR;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Loans.Return
{
    public class ReturnLoanHandler : IRequestHandler<ReturnLoanCommand, ReturnLoanResponse>
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IResourceRepository _resourceRepo;
        private readonly IPenaltyRepository _penaltyRepo;

        public ReturnLoanHandler(
            ILoanRepository loanRepo, 
            IResourceRepository resourceRepo, 
            IPenaltyRepository penaltyRepo)
        {
            _loanRepo = loanRepo;
            _resourceRepo = resourceRepo;
            _penaltyRepo = penaltyRepo;
        }

        public async Task<ReturnLoanResponse> Handle(ReturnLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepo.GetByIdAsync(request.LoanId)
                ?? throw new KeyNotFoundException("Préstamo no encontrado.");

            if (loan.Status != LoanStatus.Active)
                throw new InvalidOperationException("Préstamo no activo.");

            // marcar devolución
            loan.Return(request.ReturnDate.ToUniversalTime());
            await _loanRepo.UpdateAsync(loan);

            // actualizar recurso
            var resource = await _resourceRepo.GetByIdAsync(loan.ResourceId);
            if (resource != null)
            {
                resource.MarkAsReturned();
                await _resourceRepo.UpdateAsync(resource);
            }

            Guid? penaltyId = null;
            bool wasLate = loan.IsOverdue(request.ReturnDate);

            // si estuvo retrasado → crear sanción
            if (wasLate)
            {
                var penalty = new Penalty(
                    loan.UserId,
                    "Devolución tardía",
                    DateTime.UtcNow.AddDays(7)
                );


                penaltyId = penalty.Id;
                await _penaltyRepo.AddAsync(penalty);
            }

            // regresar DTO
            return new ReturnLoanResponse
            {
                LoanId = loan.Id,
                ReturnedAt = request.ReturnDate,
                WasLate = wasLate,
                PenaltyId = penaltyId
            };
        }
    }
}
