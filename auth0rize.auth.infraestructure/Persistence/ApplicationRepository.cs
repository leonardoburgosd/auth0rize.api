using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.infraestructure.Extensions;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class ApplicationRepository : IApplicationRepository
    {
        public ApplicationRepository() { }

        public async Task<List<ApplicationGet>> get(int userId)
        {
            List<ApplicationGet> response = new List<ApplicationGet>();

            LocalData.applicationUsers.Where(au => au.User == userId).ToList().ForEach(au =>
            {
                Application? application = LocalData.applications.FirstOrDefault(a => a.Id == au.Application);

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
    }
}
