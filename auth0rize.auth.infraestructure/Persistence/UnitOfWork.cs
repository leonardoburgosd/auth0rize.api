using auth0rize.auth.domain.Primitives;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly NpgsqlConnection _connection;
        
        public UnitOfWork(IConfiguration configuration)
        {
            string? connectionString = Environment.GetEnvironmentVariable(configuration["connection:postgres"]!.ToString());
            _connection = new NpgsqlConnection(connectionString);
        }

        public IDbConnection Connection => _connection;

        public Task<int> Complete()
        {
            return Task.FromResult(0);
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new GenericRepository<T>(_connection);
                _repositories.Add(type, repoInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public void Dispose()
        {
            _connection.Dispose();
            _connection?.Dispose();
        }
    }
}
