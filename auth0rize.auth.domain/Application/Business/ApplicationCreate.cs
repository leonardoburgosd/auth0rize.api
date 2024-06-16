namespace auth0rize.auth.domain.Application.Business
{
    public class ApplicationCreate
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Avatar { get; set; }
        public int Type { get; set; }
        public int Domain { get; set; }
        public int UserRegistration { get; set; }
    }
}
