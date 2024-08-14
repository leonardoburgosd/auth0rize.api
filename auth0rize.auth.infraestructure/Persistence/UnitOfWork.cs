using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;
using System.Data;

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

        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();
            User = new UserRepository(_connection, _transaction);
            Application = new ApplicationRepository(_connection, _transaction);
            Domain = new DomainRepository(_connection, _transaction);
            TypeUser = new TypeUserRepository(_connection, _transaction);
        }
        public void BeginTransaction()
        {
            if (_transaction == null)
                _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
        }
    }
}
