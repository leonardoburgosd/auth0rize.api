namespace auth0rize.auth.application.Features.Autentication.Queries.Login
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Application { get; set; }
    }
}
