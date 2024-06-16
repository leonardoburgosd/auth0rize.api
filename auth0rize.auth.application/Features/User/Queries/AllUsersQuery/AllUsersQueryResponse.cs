namespace auth0rize.auth.application.Features.User.Queries.AllUsersQuery
{
    public class AllUsersQueryResponse
    {
        public long Id { get; set; } = 0!;
        public string UserName { get; set; } = null!;
        public long TypeUser { get; set; }
        public string TypeUserName { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MotherLastName { get; set; } = null!;
        public bool IsDoubleFactorActivate { get; set; }
        public string Avatar { get; set; } = null!;
        public long Domain { get; set; } = 0!;
        public string DomainName { get; set; } = null!;
        public long Application { get; set; } = 0!;
        public string ApplicationName { get; set; }
    }
}
