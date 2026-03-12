namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGet
{
    public class ApplicationGetResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public List<ApplicationListItem> Applications { get; set; } = new();
    }

    public class ApplicationListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public bool IsDeleted { get; set; }
        public string RegistrationDate { get; set; }
    }
}
