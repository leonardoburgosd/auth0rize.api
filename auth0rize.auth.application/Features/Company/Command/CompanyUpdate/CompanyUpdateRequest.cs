namespace auth0rize.auth.application.Features.Company.Command.CompanyUpdate
{
    public class CompanyUpdateRequest
    {
        public string Name { get; set; }
        public int DomainId { get; set; }
        public string? Avatar { get; set; }
    }
}
