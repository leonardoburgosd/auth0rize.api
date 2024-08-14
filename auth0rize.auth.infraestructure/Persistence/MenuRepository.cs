using auth0rize.auth.domain.Menu;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public MenuRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }
    }
}
