namespace auth0rize.auth.application.Features.User.Update.UserUpdate
{
    public class UserUpdateRequest
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
    }
}
