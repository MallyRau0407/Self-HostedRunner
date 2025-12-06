using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Project.Domain.IRepositories;

namespace Project.Infrastructure.Data
{
    internal sealed class ProjectDb : IProjectDb
    {
        private readonly IConfiguration _configuration;

        public ProjectDb(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }
    }
}
