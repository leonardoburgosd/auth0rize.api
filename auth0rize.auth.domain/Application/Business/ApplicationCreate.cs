namespace auth0rize.auth.domain.Application.Business
{
    public class ApplicationCreate
    {
        public string Name { get; set; }
        public Guid Code { get; set; }
        public string Description { get; set; }
        public long Userregistration { get; set; }
    }
}
