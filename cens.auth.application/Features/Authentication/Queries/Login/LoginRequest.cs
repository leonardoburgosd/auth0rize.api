namespace cens.auth.application.Features.Authentication.Queries.Login
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
    }
}
