using System.Data;

namespace Project.Domain.IRepositories
{
    public interface IProjectDb
    {
        IDbConnection GetConnection();
    }
}
