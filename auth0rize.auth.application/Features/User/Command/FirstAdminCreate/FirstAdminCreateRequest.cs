namespace auth0rize.auth.application.Features.User.Command.FirstAdminCreate
{
    public class FirstAdminCreateRequest
    {
        public string Name { get; set; }
        public string MotherLastName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class FirstAdminCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MotherLastName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
