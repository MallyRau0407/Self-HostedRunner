using System.Data;
using Dapper;
using Project.Domain.Entities;
using Project.Domain.IRepositories;

namespace Project.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IProjectDb _db;
        public LoanRepository(IProjectDb db) => _db = db;

        public async Task AddAsync(Loan loan)
        {
            const string sql = @"
INSERT INTO Loans (Id, UserId, ResourceId, StartDate, DueDate, ReturnDate, Status)
VALUES (@Id, @UserId, @ResourceId, @StartDate, @DueDate, @ReturnDate, @Status);";

            using var conn = _db.GetConnection();
            // Si tu GetConnection devuelve conexión cerrada, Dapper la abre automáticamente al ejecutar.
            await conn.ExecuteAsync(sql, new
            {
                Id = loan.Id,
                UserId = loan.UserId,
                ResourceId = loan.ResourceId,
                StartDate = loan.StartDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                Status = (int)loan.Status
            });
        }

        public async Task<Loan?> GetActiveByResourceIdAsync(Guid resourceId)
        {
            const string sql = @"SELECT TOP 1 * FROM Loans WHERE ResourceId = @ResourceId AND Status = @Active";
            using var conn = _db.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Loan>(sql, new { ResourceId = resourceId, Active = (int)Project.Domain.Enums.LoanStatus.Active });
        }

        public async Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId, int page, int pageSize)
        {
            const string sql = @"
SELECT * FROM Loans
WHERE UserId = @UserId
ORDER BY StartDate DESC
OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
            using var conn = _db.GetConnection();
            return await conn.QueryAsync<Loan>(sql, new { UserId = userId, Skip = (page - 1) * pageSize, Take = pageSize });
        }

        public async Task<IEnumerable<Loan>> GetAllAsync(int page, int pageSize)
        {
            const string sql = @"
SELECT * FROM Loans
ORDER BY StartDate DESC
OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";
            using var conn = _db.GetConnection();
            return await conn.QueryAsync<Loan>(sql, new { Skip = (page - 1) * pageSize, Take = pageSize });
        }

        public async Task UpdateAsync(Loan loan)
        {
            const string sql = @"
UPDATE Loans
SET ReturnDate = @ReturnDate,
    Status = @Status
WHERE Id = @Id;";
            using var conn = _db.GetConnection();
            await conn.ExecuteAsync(sql, new { ReturnDate = loan.ReturnDate, Status = (int)loan.Status, Id = loan.Id });
        }

        public async Task<Loan?> GetByIdAsync(Guid id)
        {
            const string sql = @"SELECT TOP 1 * FROM Loans WHERE Id = @Id";
            using var conn = _db.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Loan>(sql, new { Id = id });
        }
    }
}
