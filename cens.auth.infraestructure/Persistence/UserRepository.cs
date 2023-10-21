using cens.auth.domain.User;
using cens.auth.domain.User.Business;
using cens.auth.infraestructure.StoredProcedures;
using Dapper;
using System.Data;


namespace cens.auth.infraestructure.Persistence
{

    public class UserRepository : IUserRepository
    {
        #region Inyeccion
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<int> create(UserCreate user)
        {
            return await _connection.ExecuteAsync(UserProcedure.create, user, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDetail?> get(string userName, string key)
        {
            return await _connection.QueryFirstOrDefaultAsync<UserDetail>(UserProcedure.getByKey,
                new
                {
                    @UserName = userName,
                    @Key = key
                },
            commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<UserGet>> get()
        {
            return await _connection.QueryAsync<UserGet>(UserProcedure.get,
                                                        null,
                                                        commandType: CommandType.StoredProcedure
                                                      );
        }

        public async Task update(UserUpdate user)
        {
            await _connection.ExecuteAsync(UserProcedure.update, user, commandType: CommandType.StoredProcedure);
        }

    }
}