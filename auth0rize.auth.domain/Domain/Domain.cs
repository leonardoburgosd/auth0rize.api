namespace auth0rize.auth.domain.Domain
{
    public class Domain : BaseEntity
    {
        public Guid Code { get; set; } = new Guid();

        public virtual ICollection<UserDomain.UserDomain> UsersDomains { get; set; }
        public virtual ICollection<Company.Company> Companies { get; set; }
    }
}