using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.Application.Queries.GetApplication
{
    public record GetApplicationQuery() : IRequest<Response<List<GetApplicationQueryResponse>>>;
}