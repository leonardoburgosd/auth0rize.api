namespace auth0rize.auth.application.Features.User.Queries.UserGetById
{
    public class UserGetByIdResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }
        public string LastLogin { get; set; }
        public int TypeId { get; set; }
    }
}
