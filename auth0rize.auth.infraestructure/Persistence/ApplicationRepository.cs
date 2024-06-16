using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.domain.ApplicationUser;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User;
using auth0rize.auth.infraestructure.Extensions;
using Dapper;
using Npgsql;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class ApplicationRepository : IApplicationRepository
    {
        #region Inyeccion
        private readonly NpgsqlConnection _connection;
        public ApplicationRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<List<ApplicationGet>> get(long userId)
        {
            List<ApplicationGet> response = null;

            User? user = LocalData.users
                               .Where(u => u.Id == userId)
                               .FirstOrDefault();

            if (user is null)
                return null;

            List<ApplicationDomain> applicationDomain = LocalData.applicationDomain.Where(ad => ad.Domain == user!.Domain).ToList();

            response = new List<ApplicationGet>();

            applicationDomain.ForEach(domain =>
            {
                Application? application = LocalData.applications.FirstOrDefault(a => a.Id == domain.Application);

                if (application is not null)
                    response.Add(new ApplicationGet()
                    {
                        Code = application.Code,
                        Description = application.Description,
                        Name = application.Name
                    });
            });

            return response;
        }

        public async Task<int> create(ApplicationCreate application)
        {
            GenericResponseCreate response = await _connection.QueryFirstAsync<GenericResponseCreate>(
                ApplicationProcedures.CREATE,
                application,
                commandType: CommandType.StoredProcedure
                );
            return response.Id;
        }
    }
}
