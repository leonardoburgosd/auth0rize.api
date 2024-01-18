using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;
using Microsoft.Extensions.Configuration;
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

        private readonly IDbConnection _connection;

        public UnitOfWork(IConfiguration configuration)
        {
            User = new UserRepository();
            Application = new ApplicationRepository();
        }

        public void Dispose()
        {
            //_connection.Dispose();
        }
    }
}
