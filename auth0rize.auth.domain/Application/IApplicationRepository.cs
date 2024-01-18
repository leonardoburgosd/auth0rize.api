using auth0rize.auth.domain.Application.Business;

namespace auth0rize.auth.domain.Application
{
    public interface IApplicationRepository
    {
        Task<List<ApplicationGet>> get(int userId);
    }
}
