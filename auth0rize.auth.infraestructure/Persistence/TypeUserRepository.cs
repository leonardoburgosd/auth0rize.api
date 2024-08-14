using auth0rize.auth.domain.TypeUser;
using Dapper;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class TypeUserRepository : ITypeUserRepository
    {
        #region Inyeccion
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public TypeUserRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }
        #endregion

        public async Task<TypeUser?> get(long id)
        {
            TypeUser? type = await _connection.QueryFirstOrDefaultAsync<TypeUser>(TypeUserConsulting.GET, transaction: _transaction);
            if (type == null) return null;
            return type;
        }
    }
}
