using Project.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> IsValidUserCredentialsAsync(string username, string password, CancellationToken cancellationToken);
        Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);
        Task<int> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken);
        Task<bool> DisableUserAsync(int userId, CancellationToken cancellationToken);
        Task<bool> UpdateUserAsync(int userId, string? fullName, string? passwordHash, CancellationToken cancellationToken);
    }
}
