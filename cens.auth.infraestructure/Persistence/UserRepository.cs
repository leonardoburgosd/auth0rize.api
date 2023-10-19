using cens.auth.domain.User;
using cens.auth.domain.User.Business;
using Dapper;
using System.Data;


namespace cens.auth.infraestructure.Persistence
{

    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> create(UserCreate user)
        {
            return await _connection.ExecuteAsync("identity.User_Create", user, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDetail?> get(string userName, string key)
        {
            return await _connection.QueryFirstOrDefaultAsync<UserDetail>("identity.User_GetByKey",
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
            return await _connection.QueryAsync<UserGet>("identity.User_Get",
                                                        null,
                                                        commandType: CommandType.StoredProcedure
                                                      );
        }
    }
}