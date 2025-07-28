namespace auth0rize.auth.domain.Login
{
    public class Login
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
