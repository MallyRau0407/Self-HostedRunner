using Dapper;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace Project.Infrastructure.Repositories
{
    public class PenaltyRepository : IPenaltyRepository
    {
        private readonly IProjectDb _db;
        public PenaltyRepository(IProjectDb db) => _db = db;

        public async Task<bool> UserHasActivePenaltyAsync(Guid userId)
        {
            const string sql = "SELECT COUNT(1) FROM Penalties WHERE UserId = @UserId AND (EndsAt IS NULL OR EndsAt > SYSUTCDATETIME())";
            using var conn = _db.GetConnection();
            var count = await conn.ExecuteScalarAsync<int>(sql, new { UserId = userId });
            return count > 0;
        }

        public async Task AddAsync(Penalty penalty)
        {
            const string sql = @"
INSERT INTO Penalties (Id, UserId, Reason, CreatedAt, EndsAt)
VALUES (@Id, @UserId, @Reason, @CreatedAt, @EndsAt);";
            using var conn = _db.GetConnection();
            await conn.ExecuteAsync(sql, new
            {
                Id = penalty.Id,
                UserId = penalty.UserId,
                Reason = penalty.Reason,
                CreatedAt = penalty.CreatedAt,
                EndsAt = penalty.EndsAt
            });
        }
    }
}
