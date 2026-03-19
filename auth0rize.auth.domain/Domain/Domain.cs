using System.ComponentModel.DataAnnotations.Schema;

namespace auth0rize.auth.domain.Domain
{
    public class Domain : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Code { get; set; }

        public string? HtmlTemplate { get; set; }
        public string? CssTemplate { get; set; }

        public virtual ICollection<UserDomain.UserDomain> UsersDomains { get; set; }
        public virtual ICollection<Company.Company> Companies { get; set; }
    }
}