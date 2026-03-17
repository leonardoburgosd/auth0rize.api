using auth0rize.auth.application.Wrappers;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Queries.ApplicationGet
{
    public record ApplicationGet(
        string? Search,
        int Page = 1,
        int Size = 10
    ) : IRequest<Response<ApplicationGetResponse>>;
}
