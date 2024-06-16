using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using auth0rize.auth.domain.User.Business;
using auth0rize.auth.infraestructure.Extensions;
using Dapper;
using Npgsql;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        #region Inyeccion
        private readonly NpgsqlConnection _connection;

        public UserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<long?> create(UserCreate user)
        {

            string parameterValues = Parameters.GetPropertyNamesAsString<UserCreate>();
            string parameterRows = parameterValues.Replace("@", "");
            string consult = UserConsulting.CREATE.Replace(EConsulting.parametersRows, parameterRows.ToLower())
                                                         .Replace(EConsulting.parametersValues, parameterValues);
            return await _connection.ExecuteScalarAsync<long>(consult, user);
        }

        public async Task<UserDetail?> get(string userName)
        {
            string consult = UserConsulting.GET_BY_USERNAME.Replace("[username]", $"{userName}");

            UserDetail? user = await _connection.QueryFirstOrDefaultAsync<UserDetail>(consult);

            return user;
        }

        public async Task<IEnumerable<UserDetail>> get(long userId)
        {
            IEnumerable<UserDetail> response = null;

            User? user = LocalData.users
                               .Where(u => u.Id.Equals(userId))
                               .FirstOrDefault();
            if (user is null)
                return response;

            response = LocalData.users.Where(u => u.Domain == user.Domain).ToList().Select(u => new UserDetail()
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password,
                Salt = u.Salt,
                Name = u.Name,
                LastName = u.LastName,
                MotherLastName = u.MotherLastName,
                Email = u.Email,
                Avatar = u.Avatar,
                IsDoubleFactorActivate = u.IsDoubleFactorActivate,
                Domain = u.Domain,
                DomainName = LocalData.domains.First(d => d.Id == u.Domain).Name,
                TypeUser = u.Type,
                TypeUserName = LocalData.types.First(t => t.Id == u.Type).Name,
                Application = LocalData.applicationDomain.First(ad => ad.Domain == u.Domain).Application,
                ApplicationName = LocalData.applications.First(a => a.Id == user.Domain).Name
            });

            return response;
        }

        public async Task<bool> userNameExist(string userName)
        {
            string consult = UserConsulting.COUNT_BY_USERNAME.Replace("[username]", $"{userName}");
            long? countUserName = await _connection.ExecuteScalarAsync<long>(consult);
            if (countUserName is null || countUserName == 0) return false;
            return true;
        }

        public async Task<bool> emailExist(string email)
        {
            string consult = UserConsulting.COUNT_BY_EMAIL.Replace("[email]", $"{email}");
            long? countEmail = await _connection.ExecuteScalarAsync<long>(consult);
            if (countEmail is null || countEmail == 0) return false;
            return true;
        }
    }
}