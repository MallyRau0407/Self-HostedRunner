using Common.CleanArch;
using Microsoft.Extensions.Logging;
using Project.Application.UseCases.Users.UserLogin.Responses;
using Project.Domain.IRepositories;

namespace Project.Application.UseCases.Users.UserLogin
{
    public sealed class UserLoginHandler : IInteractor<UserLoginRequest, UserLoginResponse>
    {
        private readonly IUserRepository _usersRepository;
        private readonly ILogger<UserLoginHandler> _logger;

        public UserLoginHandler(IUserRepository usersRepository, ILogger<UserLoginHandler> logger)
        {
            _usersRepository = usersRepository;
            _logger = logger;
        }

        public async Task<UserLoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Intento de login para usuario {User}", request.User);

            var isValid = await _usersRepository
                .IsValidUserCredentialsAsync(request.User, request.Password, cancellationToken)
                .ConfigureAwait(false);

            if (!isValid)
            {
                _logger.LogWarning("Login fallido para usuario {User}", request.User);
                return new FailureUserLoginResponse("Usuario o contraseña incorrectos.");
            }

            _logger.LogInformation("Login exitoso para usuario {User}", request.User);
            // Aquí podrías devolver más info (claims, token, etc.) si lo necesitas
            return new SuccessUserLoginResponse("Login exitoso.");
        }
    }
}
