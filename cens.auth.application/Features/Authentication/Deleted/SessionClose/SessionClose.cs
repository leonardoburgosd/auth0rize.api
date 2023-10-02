using cens.auth.application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace cens.auth.application.Features.Authentication.Deleted.SessionClose
{
    public class SessionClose : IRequest<Response<bool>>
    {
        public HttpContext HttpContext { get; set; }
    }

    public class SessionCloseHandler : IRequestHandler<SessionClose, Response<bool>>
    {
        public async Task<Response<bool>> Handle(SessionClose request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            request.HttpContext.Session.Clear();
            response.Success = true;
            response.Message = "Session cerrada.";

            return response;
        }
    }
}
