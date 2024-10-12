namespace auth0rize.auth.application.Features.Domain.Queries.GetAllByIdQuery
{
    public class GetAllByIdQueryResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid Code { get; set; }
        public string Description { get; set; }
        public long DomainId { get; set; }
        public string DomainName { get; set; }
    }
}
