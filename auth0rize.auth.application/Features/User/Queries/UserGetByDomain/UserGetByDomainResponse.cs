namespace auth0rize.auth.application.Features.User.Queries.UserGetByDomain
{
    public class UserGetByDomainResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public List<UserByDomainListResponse> Users { get; set; }
    }

    public class UserByDomainListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string LastLogin { get; set; }
    }
}
