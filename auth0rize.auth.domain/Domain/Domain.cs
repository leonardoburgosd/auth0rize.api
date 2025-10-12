namespace auth0rize.auth.domain.Domain
{
    public class Domain : BaseEntity
    {
        public Guid Code { get; set; } = Guid.NewGuid();

        public virtual ICollection<UserDomain.UserDomain> UsersDomains { get; set; }
        public virtual ICollection<Company.Company> Companies { get; set; }
    }
}