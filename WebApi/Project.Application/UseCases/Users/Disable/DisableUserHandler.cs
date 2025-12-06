using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.Common;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.Disable
{
    public sealed class DisableUserHandler : IInteractor<DisableUserRequest, UserCommandResponse>
    {
        private readonly IUserRepository _usersRepository;
        private readonly ILogger<DisableUserHandler> _logger;

        public DisableUserHandler(IUserRepository usersRepository, ILogger<DisableUserHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCommandResponse> Handle(DisableUserRequest request, CancellationToken cancellationToken)
        {
            var existing = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (existing is null)
            {
                return new FailureUserCommandResponse("Usuario no encontrado.", 404);
            }

            if (!existing.IsActive)
            {
                return new SuccessUserCommandResponse(
                    existing.Id,
                    existing.UserName,
                    existing.FullName,
                    false,
                    "El usuario ya estaba deshabilitado.");
            }

            var disabled = await _usersRepository.DisableUserAsync(request.UserId, cancellationToken);

            if (!disabled)
            {
                return new FailureUserCommandResponse("No se pudo deshabilitar el usuario.");
            }

            return new SuccessUserCommandResponse(
                existing.Id,
                existing.UserName,
                existing.FullName,
                false,
                "Usuario deshabilitado correctamente."
            );
        }
    }
}
