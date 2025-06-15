namespace auth0rize.auth.application.Features.User.Command.UserCreate
{
    public class UserCreateRequest
    {
        public string Name { get; set; }
        public string MotherLastName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int TypeUserId { get; set; }
        public int DomainId { get; set; }
    }

    public class UserCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MotherLastName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int TypeUserId { get; set; }
        public int DomainId { get; set; }
    }
}
