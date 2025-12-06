using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.Common;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.Update
{
    public sealed class UpdateUserHandler : IInteractor<UpdateUserRequest, UserCommandResponse>
    {
        private readonly IUserRepository _usersRepository;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUserRepository usersRepository, ILogger<UpdateUserHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCommandResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var existing = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (existing is null)
            {
                return new FailureUserCommandResponse("Usuario no encontrado.", 404);
            }

            var updated = await _usersRepository
                .UpdateUserAsync(request.UserId, request.FullName, request.Password, cancellationToken);

            if (!updated)
            {
                return new FailureUserCommandResponse("No se pudo actualizar el usuario.");
            }

            _logger.LogInformation("Usuario {Id} actualizado", request.UserId);

            return new SuccessUserCommandResponse(
                request.UserId,
                existing.UserName,
                request.FullName ?? existing.FullName,
                existing.IsActive,
                "Usuario actualizado correctamente."
            );
        }
    }
}
