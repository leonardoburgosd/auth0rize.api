namespace auth0rize.auth.domain.User
{
    public interface IUserNotificationRepository
    {
        Task Registration(string url, string to);
        Task RegistrationConfirm(string url,string to);
        Task RegistrationError(string url,string to);
        Task LoginCorrect(string to);
    }
}
