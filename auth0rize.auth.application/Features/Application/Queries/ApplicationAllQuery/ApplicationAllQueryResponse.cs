namespace auth0rize.auth.application.Features.Application.Queries.ApplicationAllQuery
{
    public class ApplicationAllQueryResponse
    {
        public long Id { get; set; }
        public Guid Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
