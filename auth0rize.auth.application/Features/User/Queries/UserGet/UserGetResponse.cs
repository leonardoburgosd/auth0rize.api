namespace auth0rize.auth.application.Features.User.Queries.UserGet
{
    public class UserGetResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Active { get; set; }
        public int Pending { get; set; }
        public int Confirmed { get; set; }
        public List<UserListResponse> Users { get; set; }
    }

    public class UserListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }
        public string LastLogin { get; set; }
        public string Type { get; set; }
    }
}
