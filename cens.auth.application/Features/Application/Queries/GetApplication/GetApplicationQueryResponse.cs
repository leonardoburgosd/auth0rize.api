
namespace cens.auth.application.Features.Application.Queries.GetApplication
{
    public class GetApplicationQueryResponse
    {
        public int ApplicationId { get; set; }
        public string Icon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}