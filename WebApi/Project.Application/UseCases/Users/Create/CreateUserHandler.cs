using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.Common;
using Project.Domain.Entities;
using Project.Domain.Enums;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.Create
{
    public sealed class CreateUserHandler : IInteractor<CreateUserRequest, UserCommandResponse>
    {
        private readonly IUserRepository _usersRepository;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IUserRepository usersRepository, ILogger<CreateUserHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserCommandResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            if (await _usersRepository.UsernameExistsAsync(request.UserName, cancellationToken))
            {
                _logger.LogWarning("No se puede crear el usuario {User} porque ya existe", request.UserName);
                return new FailureUserCommandResponse("El usuario ya existe.", 409);
            }

            // Crear usuario usando el constructor CORRECTO
            var user = new User(
                request.UserName,
                request.FullName,
                request.Password,
                UserRole.Basic // Usa el rol por defecto o el que tú quieras
            );

            var newId = await _usersRepository.CreateUserAsync(user, cancellationToken);

            if (newId <= 0)
            {
                _logger.LogError("No se pudo crear el usuario {User}", request.UserName);
                return new FailureUserCommandResponse("No se pudo crear el usuario.");
            }

            _logger.LogInformation("Usuario {User} creado con id {Id}", request.UserName, newId);

            return new SuccessUserCommandResponse(
                newId,
                user.UserName,
                user.FullName,
                user.IsActive,
                "Usuario creado correctamente.");
        }
    }
}
