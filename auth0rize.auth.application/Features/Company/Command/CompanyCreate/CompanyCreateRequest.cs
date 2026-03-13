namespace auth0rize.auth.application.Features.Company.Command.CompanyCreate
{
    public class CompanyCreateRequest
    {
        public string Name { get; set; }
        public string DomainId { get; set; }
        public string? Avatar { get; set; }
    }

    public class CompanyCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DomainCode { get; set; }
        public string? Avatar { get; set; }
    }
}
