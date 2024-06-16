namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    public class SuperUserRegisterRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
