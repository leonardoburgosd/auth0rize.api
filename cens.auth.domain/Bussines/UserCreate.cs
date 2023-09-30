namespace cens.auth.domain.Bussines
{
    public class UserCreate
    {
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime Birthday { get; set; }
        public string RegistrationUser { get; set; }
    }
}
