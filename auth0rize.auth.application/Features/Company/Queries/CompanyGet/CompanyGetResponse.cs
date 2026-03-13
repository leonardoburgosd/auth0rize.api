namespace auth0rize.auth.application.Features.Company.Queries.CompanyGet
{
    public class CompanyGetResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public List<CompanyListResponse> Companies { get; set; }
    }

    public class CompanyListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DomainId { get; set; }
        public string DomainCode { get; set; }
        public string? Avatar { get; set; }
        public string RegistrationDate { get; set; }
    }
}
