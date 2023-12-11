
namespace cens.auth.application.Features.Application.Command.CreateApplication
{
    public class CreateApplicationCommandRequest
    {
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}