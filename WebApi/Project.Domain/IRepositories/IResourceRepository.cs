using Project.Domain.Entities;

namespace Project.Domain.IRepositories
{
    public interface IResourceRepository
    {
        Task<Resource?> GetByIdAsync(Guid id);
        Task UpdateAsync(Resource resource);
    }
}
