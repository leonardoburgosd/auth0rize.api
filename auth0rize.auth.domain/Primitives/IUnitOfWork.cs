namespace auth0rize.auth.domain.Primitives
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> Complete();
    }
}
