using System.ComponentModel.DataAnnotations.Schema;

namespace auth0rize.auth.domain.Application
{
    public class Application : BaseEntity
    {
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Code { get; set; }

        public string Description { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<ApplicationCompany.ApplicationCompany> ApplicationsCompanies { get; set; }
    }
}
