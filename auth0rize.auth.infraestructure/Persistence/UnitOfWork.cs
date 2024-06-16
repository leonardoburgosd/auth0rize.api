﻿using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
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
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
