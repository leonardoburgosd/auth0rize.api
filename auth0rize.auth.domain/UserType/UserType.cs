namespace auth0rize.auth.domain.UserType
{
    public class UserType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<User.User> Users { get; set; }
        public virtual ICollection<UserDomain.UserDomain> UserDomains { get; set; }
    }
}
