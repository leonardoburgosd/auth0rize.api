namespace auth0rize.auth.domain.Company
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string DomainId { get; set; }
        public string Avatar { get; set; }

        public virtual Domain.Domain Domain { get; set; }
        public virtual ICollection<ApplicationCompany.ApplicationCompany> ApplicationsCompanies { get; set; }
    }
}
