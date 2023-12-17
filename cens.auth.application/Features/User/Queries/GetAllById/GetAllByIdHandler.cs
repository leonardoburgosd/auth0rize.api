using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cens.auth.application.Wrappers;
using cens.auth.domain.Primitives;
using cens.auth.domain.User.Business;
using MediatR;

namespace cens.auth.application.Features.User.Queries.GetAllById
{
    public class GetAllByIdHandler : IRequestHandler<GetAllById, Response<GetAllByIdResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public GetAllByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion
        public async Task<Response<GetAllByIdResponse>> Handle(GetAllById request, CancellationToken cancellationToken)
        {
            Response<GetAllByIdResponse> response = new Response<GetAllByIdResponse>();
            UserGetById userDetail = await _unitOfWork.User.get(request.userId);

            return response;
        }
    }
}