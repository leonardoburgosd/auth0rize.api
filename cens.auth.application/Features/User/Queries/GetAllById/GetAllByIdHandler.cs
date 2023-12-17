using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cens.auth.application.Common;
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
            if (userDetail is null)
            {
                response.Success = false;
                response.Message = Message.NODATA;
            }
            IEnumerable<domain.Application.Application> applicationsByUser = await _unitOfWork.Application.get(request.userId);
            List<ApplicationsByUser> applications = new List<ApplicationsByUser>();
            if (applicationsByUser is not null)
            {
                applicationsByUser.ToList().ForEach(abu =>
                {
                    applications.Add(new ApplicationsByUser()
                    {
                        Id = abu.Id,
                        Icon = abu.Icon,
                        Key = abu.Key,
                        Name = abu.Name
                    });
                });
            }

            response.Success = true;
            response.Data = new GetAllByIdResponse()
            {
                UserName = userDetail.UserName,
                Avatar = userDetail.Avatar,
                Birthday = userDetail.Birthday,
                IsDoubleFactorActive = userDetail.IsDoubleFactorActivate,
                LastName = userDetail.LastName,
                MotherLastName = userDetail.MotherLastName,
                Name = userDetail.Name,
                TypeId = userDetail.TypeId,
                Applications = applications
            };
            response.Message = Message.WHITDATA;
            return response;
        }
    }
}