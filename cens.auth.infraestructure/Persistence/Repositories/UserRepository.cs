using cens.auth.domain.Bussines;
using cens.auth.infraestructure.Persistence.Interfaces;
using Dapper;
using System.Data;

namespace cens.auth.infraestructure.Persistence.Repositories
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

        public async Task<UserDetail> get(string userName, string key)
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
    }
}
