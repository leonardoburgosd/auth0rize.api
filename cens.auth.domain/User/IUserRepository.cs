using cens.auth.domain.User.Business;

namespace cens.auth.domain.User;
public interface IUserRepository
{
    Task<UserDetail?> get(string userName, string key);
    Task<int> create(UserCreate user);
    Task delete(int userId, string userName);
    Task update(UserUpdate user);
    Task<IEnumerable<UserGet>> get();
    Task<UserGetById> get(int userId);
}
