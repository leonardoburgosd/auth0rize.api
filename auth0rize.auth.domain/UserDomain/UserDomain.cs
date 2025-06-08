namespace auth0rize.auth.domain.UserDomain
{
    public class UserDomain
    {
        public int UserId { get; set; }
        public int DomainId { get; set; }
        public int RoleId { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int UserRegistration { get; set; }
        public DateTime DateUpdate { get; set; }
        public int UserUpdate { get; set; }
        public DateTime DateDeleted { get; set; }
        public int UserDeleted { get; set; }
        public bool IsDeleted { get; set; }

        public virtual UserType.UserType Role { get; set; }
        public virtual Domain.Domain Domain { get; set; }
        public virtual User.User User { get; set; }

        public virtual Tuple<int, int> UserIdDomainId { get; set; }
    }
}
