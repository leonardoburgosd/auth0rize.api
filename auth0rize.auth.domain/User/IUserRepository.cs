using auth0rize.auth.domain.User.Business;

namespace auth0rize.auth.domain.User
{
    public interface IUserRepository
    {
        Task<UserDetail?> get(string username);
        Task<long?> create(UserCreate user);
        Task<IEnumerable<UserDetail>> get(long userId);
        Task<bool> userNameExist(string userName);
        Task<bool> emailExist(string email);
    }
}
