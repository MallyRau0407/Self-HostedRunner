using System.Data;
using Dapper;
using System.Threading;
using System.Threading.Tasks;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace Project.Infrastructure.Repositories
{
    internal sealed class UsersRepository : IUserRepository
    {
        private readonly IProjectDb _db;

        public UsersRepository(IProjectDb db)
        {
            _db = db;
        }

        public async Task<bool> IsValidUserCredentialsAsync(
            string username,
            string password,
            CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM Users
                WHERE UserName = @UserName
                  AND CONVERT(VARCHAR(64), PasswordHash, 2) = @Password
                  AND UserActive = 1;
            ";

            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new { UserName = username, Password = password }, cancellationToken: cancellationToken);
            var count = await conn.ExecuteScalarAsync<int>(cmd).ConfigureAwait(false);

            return count > 0;
        }

        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        {
            const string sql = @"SELECT COUNT(1) FROM Users WHERE UserName = @UserName;";
            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new { UserName = username }, cancellationToken: cancellationToken);
            var count = await conn.ExecuteScalarAsync<int>(cmd).ConfigureAwait(false);
            return count > 0;
        }

        public async Task<int> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO Users (UserName, PasswordHash, FullName, UserActive)
                VALUES (@UserName, CONVERT(VARBINARY(32), @PasswordHash, 2), @FullName, 1);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new
            {
                user.UserName,
                user.PasswordHash,
                user.FullName
            }, cancellationToken: cancellationToken);
            var id = await conn.ExecuteScalarAsync<int>(cmd).ConfigureAwait(false);

            return id;
        }

        public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    Id = ISNULL(UserId, IdUser),
                    UserName,
                    PasswordHash = CONVERT(VARCHAR(64), PasswordHash, 2),
                    FullName,
                    UserActive
                FROM Users
                WHERE (UserId = @UserId OR IdUser = @UserId);
            ";

            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken);
            var user = await conn.QueryFirstOrDefaultAsync<User>(cmd).ConfigureAwait(false);
            return user;
        }

        public async Task<bool> DisableUserAsync(int userId, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE Users
                SET UserActive = 0
                WHERE (UserId = @UserId OR IdUser = @UserId)
                  AND UserActive = 1;
            ";

            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new { UserId = userId }, cancellationToken: cancellationToken);
            var rows = await conn.ExecuteAsync(cmd).ConfigureAwait(false);
            return rows > 0;
        }

        public async Task<bool> UpdateUserAsync(int userId, string? fullName, string? passwordHash, CancellationToken cancellationToken)
        {
            var setClauses = new List<string>();
            if (!string.IsNullOrWhiteSpace(fullName)) setClauses.Add("FullName = @FullName");
            if (!string.IsNullOrWhiteSpace(passwordHash)) setClauses.Add("PasswordHash = CONVERT(VARBINARY(32), @PasswordHash, 2)");
            if (setClauses.Count == 0) return false;

            var sql = $@"UPDATE Users SET {string.Join(", ", setClauses)} WHERE (UserId = @UserId OR IdUser = @UserId) AND UserActive = 1;";
            using var conn = _db.GetConnection();
            var cmd = new CommandDefinition(sql, new { UserId = userId, FullName = fullName, PasswordHash = passwordHash }, cancellationToken: cancellationToken);
            var rows = await conn.ExecuteAsync(cmd).ConfigureAwait(false);
            return rows > 0;
        }
    }
}
