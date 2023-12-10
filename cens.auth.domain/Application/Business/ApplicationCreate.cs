namespace cens.auth.domain.Application.Business
{
    public class ApplicationCreate
    {
        public string Icon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string RegistrationUser { get; set; } = null!;
    }
}