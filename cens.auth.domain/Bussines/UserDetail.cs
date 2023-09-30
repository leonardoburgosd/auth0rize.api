namespace cens.auth.domain.Bussines
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MotherLastName { get; set; }
        public bool IsDoubleFactorActivate { get; set; }
        public string Avatar { get; set; }
        public string Detail { get; set; }
    }
}
