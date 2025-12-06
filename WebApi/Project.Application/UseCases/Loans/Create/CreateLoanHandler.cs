using MediatR;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Loans.Create
{
    public class CreateLoanHandler : IRequestHandler<CreateLoanCommand, Guid>
    {
        private readonly ILoanRepository _loanRepo;
        private readonly IResourceRepository _resourceRepo;
        private readonly IPenaltyRepository _penaltyRepo;

        public CreateLoanHandler(ILoanRepository loanRepo, IResourceRepository resourceRepo, IPenaltyRepository penaltyRepo)
        {
            _loanRepo = loanRepo;
            _resourceRepo = resourceRepo;
            _penaltyRepo = penaltyRepo;
        }

        public async Task<Guid> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            // 1) Validar sanciones
            if (await _penaltyRepo.UserHasActivePenaltyAsync(request.UserId))
                throw new InvalidOperationException("El usuario tiene sanción activa.");

            // 2) Verificar recurso
            var resource = await _resourceRepo.GetByIdAsync(request.ResourceId);
            if (resource is null) throw new KeyNotFoundException("Recurso no encontrado.");
            if (!resource.IsAvailable) throw new InvalidOperationException("Recurso no disponible.");

            // 3) Verificar préstamo activo para el recurso
            var active = await _loanRepo.GetActiveByResourceIdAsync(request.ResourceId);
            if (active != null) throw new InvalidOperationException("Recurso ya prestado.");

            // 4) Crear préstamo
            var loan = new Loan(request.UserId, request.ResourceId, request.StartDate.ToUniversalTime(), request.DueDate.ToUniversalTime());
            await _loanRepo.AddAsync(loan);

            // 5) Marcar recurso como prestado
            resource.MarkAsBorrowed();
            await _resourceRepo.UpdateAsync(resource);

            return loan.Id;
        }
    }
}
