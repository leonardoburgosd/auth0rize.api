using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.User;

namespace auth0rize.auth.domain.Primitives
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IApplicationRepository Application { get; }
        IDomainRepository Domain { get; }
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Complete();
    }
}
