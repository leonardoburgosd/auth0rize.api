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
            UserDetail response = new UserDetail();

            User? user = LocalData.users
                               .Where(u => u.UserName.Equals(userName))
                               .FirstOrDefault();

            if (user is null)
                return response;

            var applicationId = LocalData.applicationUsers.FirstOrDefault(au => au.User == user.Id);
            var type = LocalData.types.FirstOrDefault(u => u.Id == user.Type);

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
                Application = applicationId is not null ? LocalData.applications.FirstOrDefault(a => a.Id == applicationId.Application)!.Code : "",
                TypeUser = type!.Id,
                TypeUserName = type.Name
            };

            return response;
        }
    }
}
