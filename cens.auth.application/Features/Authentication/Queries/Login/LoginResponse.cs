namespace cens.auth.application.Features.Authentication.Queries.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string UsuarioNombre { get; set; } = null!;
    }
}
