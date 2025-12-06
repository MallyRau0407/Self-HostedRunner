using Project.Domain.Entities;

namespace Project.Domain.IRepositories
{
    public interface ILoanRepository
    {
        Task AddAsync(Loan loan);
        Task<Loan?> GetActiveByResourceIdAsync(Guid resourceId);
        Task<IEnumerable<Loan>> GetByUserIdAsync(Guid userId, int page, int pageSize);
        Task<IEnumerable<Loan>> GetAllAsync(int page, int pageSize);
        Task UpdateAsync(Loan loan);
        Task<Loan?> GetByIdAsync(Guid id);
    }
}
