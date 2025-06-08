namespace auth0rize.auth.domain.User
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MotherLastName { get; set; } = null!;

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        public string Avatar { get; set; } = "default.png";
        public int TypeId { get; set; }
        public bool IsDoubleFactorActive { get; set; } = false;
        public bool IsConfirmed { get; set; } = false;
        public string DoubleFactorActiveCode { get; set; } = "";


        public virtual UserType.UserType UserType { get; set; }

        public virtual ICollection<ConfirmAccount.ConfirmAccount> ConfirmAccounts { get; set; }
        public virtual ICollection<Domain.Domain> Domains { get; set; }
        public virtual ICollection<UserDomain.UserDomain> UsersDomains { get; set; }
        public virtual ICollection<Company.Company> Companies { get; set; }
        public virtual ICollection<Application.Application> Applications { get; set; }
        public virtual ICollection<ApplicationCompany.ApplicationCompany> ApplicationsCompanies { get; set; }
    }
}
