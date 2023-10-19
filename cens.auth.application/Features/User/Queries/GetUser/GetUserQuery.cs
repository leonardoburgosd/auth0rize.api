using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using MediatR;

namespace cens.auth.application.Features.User.Queries.GetUser
{
    public record GetUserQuery(SecurityTokenData securityTokenData) : IRequest<Response<List<GetUserResponse>>>;
}