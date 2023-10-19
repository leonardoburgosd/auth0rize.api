namespace cens.auth.application.Features.Authentication.Queries.Login
{
    public class LoginRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Key { get; set; } = null!;
    }
}
