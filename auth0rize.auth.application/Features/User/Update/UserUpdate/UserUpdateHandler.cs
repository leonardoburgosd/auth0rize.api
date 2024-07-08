using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Update.UserUpdate
{
    internal sealed class UserUpdateHandler : IRequestHandler<UserUpdate, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public UserUpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(UserUpdate request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();



            response.Success = true;
            response.Message = "Usuario actualizado correctamente.";
            response.Data = true;

            return response;
        }
    }
}
