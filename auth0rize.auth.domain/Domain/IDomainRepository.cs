using auth0rize.auth.domain.Domain.Business;

namespace auth0rize.auth.domain.Domain
{
    public interface IDomainRepository
    {
        Task<long?> create(DomainCreate domain);
        Task<bool> exist(string code);
    }
}
