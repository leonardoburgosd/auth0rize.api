namespace auth0rize.auth.domain
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int? UserRegistration { get; set; } = null;
        public DateTime DateUpdate { get; set; }
        public int UserUpdate { get; set; }
        public DateTime DateDeleted { get; set; }
        public int UserDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
