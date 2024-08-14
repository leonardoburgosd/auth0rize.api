using auth0rize.auth.domain.Option;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class OptionRepository : IOptionRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public OptionRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }
    }
}
