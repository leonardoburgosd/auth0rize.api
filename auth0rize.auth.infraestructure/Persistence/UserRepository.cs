using auth0rize.auth.domain.User;
using auth0rize.auth.domain.User.Business;
using auth0rize.auth.infraestructure.Extensions;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        public UserRepository() { }

        public async Task<UserDetail> get(string userName)
        {
            UserDetail response = null;

            User? user = LocalData.users
                               .Where(u => u.UserName.Equals(userName))
                               .FirstOrDefault();

            if (user is null)
                return response;

            var type = LocalData.types.FirstOrDefault(u => u.Id == user.Type);
            long application = LocalData.applicationDomain.First(ad => ad.Domain == user.Domain).Application;

            response = new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password,
                Salt = user.Salt,
                Name = user.Name,
                LastName = user.LastName,
                MotherLastName = user.MotherLastName,
                Email = user.Email,
                Avatar = user.Avatar,
                IsDoubleFactorActivate = user.IsDoubleFactorActivate,
                Domain = user.Domain,
                DomainName = LocalData.domains.First(d => d.Id == user.Domain).Name,
                TypeUser = type!.Id,
                TypeUserName = type.Name,
                Application = application,
                ApplicationName = LocalData.applications.First(a => a.Id == application).Name
            };

            return response;
        }
    }
}
