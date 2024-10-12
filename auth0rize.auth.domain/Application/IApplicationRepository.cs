using auth0rize.auth.domain.Application.Business;

namespace auth0rize.auth.domain.Application
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationGet>?> get(long userId);
        Task<long?> create(ApplicationCreate application);
        Task deleted(long id, long userId);
        Task<ApplicationGet?> getById(long id);
    }
}
