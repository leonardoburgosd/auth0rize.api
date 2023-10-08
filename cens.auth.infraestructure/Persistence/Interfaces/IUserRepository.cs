using cens.auth.domain.Bussines;

namespace cens.auth.infraestructure.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDetail> get(string userName, string key);
        Task<int> create(UserCreate user);
        Task<IEnumerable<UserGet>> get();
    }
}
