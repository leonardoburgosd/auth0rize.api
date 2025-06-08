namespace auth0rize.auth.domain.User
{
    public interface IUserNotificationRepository
    {
        Task Registration(string url, string to);
    }
}
