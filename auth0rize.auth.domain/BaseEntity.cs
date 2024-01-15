namespace auth0rize.auth.domain
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public DateTime RegistrationDate { get; set; }
        public long UserRegistration { get; set; }
        public DateTime DateUpdate { get; set; }
        public long UserUpdate { get; set; }
        public DateTime DateDeleted { get; set; }
        public long UserDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
