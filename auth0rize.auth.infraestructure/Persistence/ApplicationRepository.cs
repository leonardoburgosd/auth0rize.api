using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using auth0rize.auth.infraestructure.Extensions;
using Dapper;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class ApplicationRepository : IApplicationRepository
    {
        #region Inyeccion
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public ApplicationRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }
        #endregion

        public async Task<List<ApplicationGet>?> get(long userId)
        {
            string consult = ApplicationProcedures.GET.Replace("[userId]", userId.ToString());
            IEnumerable<ApplicationGet> response = await _connection.QueryAsync<ApplicationGet>(consult, transaction: _transaction);
            return response.ToList();
        }

        public async Task<ApplicationGet?> getById(long id)
        {
            string consult = ApplicationProcedures.GET_BYID.Replace("[applicationId]", id.ToString());
            ApplicationGet? application = await _connection.QueryFirstOrDefault(consult, transaction: _transaction);
            return application;
        }

        public async Task<long?> create(ApplicationCreate application)
        {
            string parameterValues = Parameters.GetPropertyNamesAsString<ApplicationCreate>();
            string parameterRows = parameterValues.Replace("@", "");

            string consult = ApplicationProcedures.CREATE.Replace(EConsulting.parametersRows, parameterRows.ToLower())
                                                         .Replace(EConsulting.parametersValues, parameterValues);

            return await _connection.ExecuteScalarAsync<long>(consult, application, _transaction);
        }

        public async Task deleted(long id, long userId)
        {
            string consult = ApplicationProcedures.DELETED.Replace("[userId]", userId.ToString())
                                                          .Replace("[id]", id.ToString());
            await _connection.ExecuteAsync(consult, transaction: _transaction);
        }
    }
}
