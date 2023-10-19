using cens.auth.domain.User.Business;

namespace cens.auth.domain.User;
public interface IUserRepository
{
    Task<UserDetail?> get(string userName, string key);
    Task<int> create(UserCreate user);
    Task<IEnumerable<UserGet>> get();
}
