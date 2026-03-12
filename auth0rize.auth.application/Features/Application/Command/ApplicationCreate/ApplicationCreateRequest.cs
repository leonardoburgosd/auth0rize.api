namespace auth0rize.auth.application.Features.Application.Command.ApplicationCreate
{
    public class ApplicationCreateRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Avatar { get; set; } = null!;
    }
}
