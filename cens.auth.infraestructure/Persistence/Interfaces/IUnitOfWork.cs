namespace cens.auth.infraestructure.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
    }
}
