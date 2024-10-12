using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Domain.Business;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.infraestructure.Extensions;
using Dapper;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class DomainRepository : IDomainRepository
    {
        #region Inyeccion
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public DomainRepository(IDbConnection connection, IDbTransaction transaction = null)
        {
            _connection = connection;
            _transaction = transaction;
        }
        #endregion

        public async Task<long?> create(DomainCreate domain)
        {
            string parameterValues = Parameters.GetPropertyNamesAsString<DomainCreate>();
            string parameterRows = parameterValues.Replace("@", "");

            string consult = DomainConsulting.CREATE.Replace(EConsulting.parametersRows, parameterRows.ToLower())
                                                         .Replace(EConsulting.parametersValues, parameterValues);
            return await _connection.ExecuteScalarAsync<long>(consult, domain, _transaction);
        }

        public async Task<bool> exist(string code)
        {
            string consult = DomainConsulting.EXIST.Replace("[name]", code);
            long count = await _connection.ExecuteScalarAsync<long>(consult, transaction: _transaction);
            if (count == 0) return false;
            return true;
        }

        public async Task<List<DomainGetById>?> get(string code)
        {
            string consult = DomainConsulting.GET_ID.Replace("[domainCode]", code);
            IEnumerable<DomainGetById>? response = await _connection.QueryAsync<DomainGetById>(consult, transaction: _transaction);
            return response.ToList();
        }

        public async Task delete(long domainId, long userId)
        {
            string consult = DomainConsulting.DELETE.Replace("[userId]",userId.ToString()).Replace("[domainId]", domainId.ToString());
            await _connection.ExecuteAsync(consult, transaction: _transaction);
        }
    }
}
