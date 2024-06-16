namespace auth0rize.auth.domain.TypeUser
{
    public interface ITypeUserRepository
    {
        Task<TypeUser?> get(long id);
    }
}
