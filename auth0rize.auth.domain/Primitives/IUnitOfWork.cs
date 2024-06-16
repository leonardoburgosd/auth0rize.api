using auth0rize.auth.domain.Application;
using auth0rize.auth.domain.Domain;
using auth0rize.auth.domain.Menu;
using auth0rize.auth.domain.Option;
using auth0rize.auth.domain.TypeUser;
using auth0rize.auth.domain.User;

namespace auth0rize.auth.domain.Primitives
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        ITypeUserRepository TypeUser { get; }
        IOptionRepository Option { get; }
        IMenuRepository Menu { get; }
        IApplicationRepository Application { get; }
        IDomainRepository Domain { get; }
    }
}
