using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Common.DeletedApplication
{
    internal sealed class DeletedApplicationHandler : IRequestHandler<DeletedApplication, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public DeletedApplicationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(DeletedApplication request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            await _unitOfWork.Application.deleted(request.idApplication, request.session.Id);

            response.Success = true;
            response.Message = "Aplicación eliminada correcta.";
            response.Data = true;

            return response;
        }
    }
}
