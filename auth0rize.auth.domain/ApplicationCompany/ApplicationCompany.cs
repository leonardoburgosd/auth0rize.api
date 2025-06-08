namespace auth0rize.auth.domain.ApplicationCompany
{
    public class ApplicationCompany
    {
        public int ApplicationId { get; set; }
        public int CompanyId { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int UserRegistration { get; set; }
        public DateTime DateUpdate { get; set; }
        public int UserUpdate { get; set; }
        public DateTime DateDeleted { get; set; }
        public int UserDeleted { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Company.Company Company { get; set; }
        public virtual Application.Application Application { get; set; }
    }
}
