namespace auth0rize.auth.domain.Domain.Business
{
    public class DomainGetById
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime Registrationdate { get; set; }
        public bool Default { get; set; }
    }
}
