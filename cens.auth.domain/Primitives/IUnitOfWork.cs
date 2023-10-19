using cens.auth.domain.User;

namespace cens.auth.domain.Primitives;
public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
}
