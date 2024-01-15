namespace auth0rize.auth.application.Features.Autentication.Queries.Login
{
    public class LoginResponse
    {
        public int DoubleFactorCode { get; set; }
        public string Token { get; set; }
    }
}
