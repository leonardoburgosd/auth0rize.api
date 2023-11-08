using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using cens.auth.domain.User.Business;
using MediatR;

namespace cens.auth.application.Features.Authentication.Queries.BasicUser
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

            UserDetail? user = await _unitOfWork.User.get(request.UserName, request.Key);

            if (user is null)
                throw new KeyNotFoundException("Usuario no se encuentra registrado.");

            response.Success = true;
            response.Message = "Usuario encontrado en la applicación.";
            response.Data = new BasicUserResponse() { Email = user.Email, Nombres = user.Name, NombreUsuario = user.UserName };

            return response;
        }
    }
}
