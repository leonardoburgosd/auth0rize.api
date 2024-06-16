using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Domain.Business;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.infraestructure.Extensions;
using Dapper;
using Npgsql;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class DomainRepository : IDomainRepository
    {
        #region Inyeccion
        private readonly NpgsqlConnection _connection;
        public DomainRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<long?> create(DomainCreate domain)
        {
            string parameterValues = Parameters.GetPropertyNamesAsString<DomainCreate>();
            string parameterRows = parameterValues.Replace("@", "");

            string consult = DomainConsulting.CREATE.Replace(EConsulting.parametersRows, parameterRows.ToLower())
                                                         .Replace(EConsulting.parametersValues, parameterValues);
            return await _connection.ExecuteScalarAsync<long>(consult, domain);
        }

        public async Task<bool> exist(string code)
        {
            string consult = DomainConsulting.EXIST.Replace("[name]", code);
            long count = await _connection.ExecuteScalarAsync<long>(consult);
            if (count == 0) return false;
            return true;
        }
    }
}
