using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User.Business;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.BasicUser
{
    internal sealed class BasicUserQueryHandler : IRequestHandler<BasicUserQuery, Response<BasicUserResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public BasicUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<BasicUserResponse>> Handle(BasicUserQuery request, CancellationToken cancellationToken)
        {
            Response<BasicUserResponse> response = new Response<BasicUserResponse>();

            UserDetail user = await _unitOfWork.User.get(request.userName);

            if (user is null) throw new KeyNotFoundException("Usuario no existe.");

            if (!string.IsNullOrEmpty(request.application) && !user.Application.Equals(request.application))
                throw new KeyNotFoundException("Usuario no tiene acceso a esta aplicación.");

            response.Success = true;
            response.Message = "Usuario encontrado.";
            response.Data = new BasicUserResponse()
            {
                Email = user.Email
            };

            return response;
        }
    }
}
