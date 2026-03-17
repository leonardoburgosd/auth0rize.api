namespace auth0rize.auth.application.Features.Company.Queries.CompanyGetById
{
    public class CompanyGetByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DomainId { get; set; }
        public string DomainCode { get; set; }
        public string? Avatar { get; set; }
        public string RegistrationDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
