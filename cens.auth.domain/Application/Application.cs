namespace cens.auth.domain.Application
{
    public class Application : EntityBase
    {
        public string Key { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Icon { get; set; } = null!;
    }
}
