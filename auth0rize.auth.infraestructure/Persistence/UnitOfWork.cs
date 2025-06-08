<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;
using Microsoft.Extensions.Configuration;
using Npgsql;
=======
=======
>>>>>>> Stashed changes
﻿using auth0rize.auth.domain.Primitives;
using System.Data;
>>>>>>> Stashed changes

namespace auth0rize.auth.infraestructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        public IUserRepository User { get; private set; }
        public ITypeUserRepository TypeUser { get; private set; }
        public IOptionRepository Option { get; private set; }
        public IMenuRepository Menu { get; private set; }
        public IApplicationRepository Application { get; private set; }
        public IDomainRepository Domain { get; private set; }
        private readonly NpgsqlConnection _connection;
        
        public UnitOfWork(IConfiguration configuration)
        {
            string? connectionString = Environment.GetEnvironmentVariable(configuration["connection:postgres"]!.ToString());
            _connection = new NpgsqlConnection(connectionString);

            User = new UserRepository(_connection);
            Application = new ApplicationRepository(_connection);
            Domain = new DomainRepository(_connection);
            TypeUser = new TypeUserRepository(_connection);
=======
=======
>>>>>>> Stashed changes

        private readonly IDbConnection _connection;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentException(nameof(connection));
            _connection.Open();
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
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }

        public void Dispose()
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            _connection.Dispose();
=======
=======
>>>>>>> Stashed changes
            _connection?.Dispose();
>>>>>>> Stashed changes
        }
    }
}
