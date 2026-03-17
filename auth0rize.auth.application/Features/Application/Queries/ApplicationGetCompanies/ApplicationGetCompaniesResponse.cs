namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGetCompanies
{
    public class ApplicationGetCompaniesResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public List<ApplicationCompanyListResponse> Companies { get; set; }
    }

    public class ApplicationCompanyListResponse
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int DomainId { get; set; }
        public string DomainCode { get; set; }
        public string? Avatar { get; set; }
        public string RegistrationDate { get; set; }
    }
}
