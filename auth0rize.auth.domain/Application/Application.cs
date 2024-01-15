namespace auth0rize.auth.domain.Application
{
    public class Application : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
