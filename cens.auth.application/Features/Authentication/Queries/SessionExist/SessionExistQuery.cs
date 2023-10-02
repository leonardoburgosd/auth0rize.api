using cens.auth.application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace cens.auth.application.Features.Authentication.Queries.SessionExist
{
    public class SessionExistQuery : IRequest<Response<SessionExistResponse>>
    {
        public HttpContext HttpContext { get; set; }
    }

    public class SessionExistQueryHandler : IRequestHandler<SessionExistQuery, Response<SessionExistResponse>>
    {
        public async Task<Response<SessionExistResponse>> Handle(SessionExistQuery request, CancellationToken cancellationToken)
        {
            Response<SessionExistResponse> response = new Response<SessionExistResponse>();

            if (request.HttpContext.Session.GetString("authCENSTknService") is null)
                throw new KeyNotFoundException("Sesion no se ah iniciado.");

            response.Success = true;
            response.Message = "Session iniciada.";
            response.Data = new SessionExistResponse()
            {
                Token = request.HttpContext.Session.GetString("authCENSTknService")
            };

            return response;
        }
    }
}
