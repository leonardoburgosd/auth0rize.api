using auth0rize.auth.domain.TypeUser;
using Dapper;
using Npgsql;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class TypeUserRepository : ITypeUserRepository
    {
        #region Inyeccion
        private readonly NpgsqlConnection _connection;

        public TypeUserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<TypeUser?> get(long id)
        {
            TypeUser? type = await _connection.QueryFirstOrDefaultAsync<TypeUser>(TypeUserConsulting.GET);
            if(type == null) return null;
            return type;
        }
    }
}
 