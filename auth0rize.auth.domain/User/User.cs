namespace auth0rize.auth.domain.User
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public bool IsDoubleFactorActivate { get; set; } = false;
        public string Avatar { get; set; } = null!;
        public int Type { get; set; }
    }
}
