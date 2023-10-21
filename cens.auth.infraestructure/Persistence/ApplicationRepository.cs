using System.Data;
using cens.auth.domain.Application;
using cens.auth.domain.Application.Business;
using cens.auth.infraestructure.StoredProcedures;
using Dapper;

namespace cens.auth.infraestructure.Persistence
{
    public class ApplicationRepository : IApplicationRepository
    {
        #region Inyeccion
        private readonly IDbConnection _connection;
        public ApplicationRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<int> create(ApplicationCreate application)
        {
            return await _connection.ExecuteAsync(ApplicationProcedure.create, application, commandType: CommandType.StoredProcedure);
        }

        public async Task delete(int applicationId, string userName)
        {
            await _connection.ExecuteAsync(ApplicationProcedure.delete, new
            {
                @ApplicationId = applicationId,
                @UserName = userName
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Application>> get()
        {
            return await _connection.QueryAsync<Application>(ApplicationProcedure.get,
                                                        null,
                                                        commandType: CommandType.StoredProcedure
                                                      );
        }

        public async Task upadte(ApplicationUpdate application)
        {
            await _connection.ExecuteAsync(ApplicationProcedure.update, application, commandType: CommandType.StoredProcedure);
        }
    }
}