namespace auth0rize.auth.domain.Option
{
    public class Option : BaseEntity
    {
        public string Name { get; set; }
        public long Parent { get; set; }
        public string Address { get; set; }
        public string Icon { get; set; }
    }
}