using Project.Domain.Entities;

namespace Project.Domain.IRepositories
{
    public interface IPenaltyRepository
    {
        Task<bool> UserHasActivePenaltyAsync(Guid userId);
        Task AddAsync(Penalty penalty);
    }
}
