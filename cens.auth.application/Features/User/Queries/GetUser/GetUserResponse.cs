namespace cens.auth.application.Features.User.Queries.GetUser
{
    public class GetUserResponse
    {
        public int Id { get; set; }
        public string NameComplete { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public List<Application> Applications { get; set; } = null!;
        public bool Deleted { get; set; }
    }

    public class Application
    {
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = string.Empty;
    }
}