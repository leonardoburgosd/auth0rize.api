using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using MediatR;

namespace cens.auth.application.Features.User.Delete.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            await _unitOfWork.User.delete(request.userId, request.SecurityTokenData.UserName);
            response.Success = true;
            response.Message = "Usuario eliminado correctamente.";
            response.Data = true;

            return response;
        }
    }
}