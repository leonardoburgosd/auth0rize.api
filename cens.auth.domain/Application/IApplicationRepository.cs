using cens.auth.domain.Application.Business;

namespace cens.auth.domain.Application
{
    public interface IApplicationRepository
    {
        Task<int> create(ApplicationCreate application);
        Task upadte(ApplicationUpdate application);
        Task<IEnumerable<Application>> get();
        Task delete(int applicationId, string userName);
    }
}