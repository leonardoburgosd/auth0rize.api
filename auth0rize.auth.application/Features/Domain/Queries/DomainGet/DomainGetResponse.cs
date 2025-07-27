namespace auth0rize.auth.application.Features.Domain.Queries.DomainGet
{
    public class DomainGetResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Active { get; set; }
        public int Deleted { get; set; }
        public List<DomainListResponse> Domains { get; set; }
    }

    public class DomainListResponse
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
        public string PrincipalName { get; set; }
        public string PrincipalEmail { get; set; }
    }
}
