﻿using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.DeleteDomain
{
    public class DeleteDomainHandler : IRequestHandler<DeleteDomain, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDomainHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(DeleteDomain request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();
            await _unitOfWork.Domain.delete(request.id, request.session.Id);

            response.Success = true;
            response.Message = "Dominio eliminado correctamente.";
            response.Data = true;

            return response;
        }
    }
}
