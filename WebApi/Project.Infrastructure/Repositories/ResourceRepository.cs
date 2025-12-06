using Dapper;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace Project.Infrastructure.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly IProjectDb _db;
        public ResourceRepository(IProjectDb db) => _db = db;

        public async Task<Resource?> GetByIdAsync(Guid id)
        {
            const string sql = "SELECT TOP 1 * FROM Resources WHERE Id = @Id";
            using var conn = _db.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Resource>(sql, new { Id = id });
        }

        public async Task UpdateAsync(Resource resource)
        {
            const string sql = "UPDATE Resources SET IsAvailable = @IsAvailable WHERE Id = @Id";
            using var conn = _db.GetConnection();
            await conn.ExecuteAsync(sql, new { IsAvailable = resource.IsAvailable, Id = resource.Id });
        }
    }
}
