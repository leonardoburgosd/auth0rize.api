namespace auth0rize.auth.domain.Application.Business
{
    public class ApplicationGet
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid Code { get; set; }
        public string Description { get; set; }
        public DateTime Registrationdate { get; set; }
    }
}
