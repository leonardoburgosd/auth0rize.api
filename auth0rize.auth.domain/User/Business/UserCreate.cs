namespace auth0rize.auth.domain.User.Business
{
    public class UserCreate
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public long Type { get; set; }
        public long Domain { get; set; }
        public long UserRegistration { get; set; }
        public bool IsDoubleFactorActivate { get; set; } = false;

    }
}
