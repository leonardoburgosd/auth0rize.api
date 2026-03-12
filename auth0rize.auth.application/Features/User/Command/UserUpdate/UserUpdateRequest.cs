namespace auth0rize.auth.application.Features.User.Command.UserUpdate
{
    public class UserUpdateRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MotherLastName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int TypeId { get; set; }
    }
}
