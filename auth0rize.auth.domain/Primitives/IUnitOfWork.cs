namespace auth0rize.auth.domain.Primitives
{
    public interface IUnitOfWork : IDisposable
    {
<<<<<<< Updated upstream
        IUserRepository User { get; }
        ITypeUserRepository TypeUser { get; }
        IOptionRepository Option { get; }
        IMenuRepository Menu { get; }
        IApplicationRepository Application { get; }
        IDomainRepository Domain { get; }
=======
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Complete();
>>>>>>> Stashed changes
    }
}
