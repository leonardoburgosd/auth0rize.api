using auth0rize.auth.domain.User.Business;

namespace auth0rize.auth.domain.User
{
    public interface IUserRepository
    {
        Task<UserDetail> get(string userName);
    }
}
