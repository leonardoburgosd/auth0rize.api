using cens.auth.application.Common;
using cens.auth.application.Common.Entities;
using cens.auth.application.Wrappers;
using cens.auth.domain.Bussines;
using cens.auth.infraestructure.Persistence.Interfaces;
using MediatR;

namespace cens.auth.application.Features.User.Queries.GetUser
{
    public class GetUserQuery : IRequest<Response<List<GetUserResponse>>>
    {
        public SecurityTokenData securityTokenData { get; set; } = null!;
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Response<List<GetUserResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GetUserResponse>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            Response<List<GetUserResponse>> response = new Response<List<GetUserResponse>>();

            IEnumerable<UserGet> users = await _unitOfWork.User.get();
            if (users is null)
            {
                response.Success = false;
                response.Message = Message.NODATA;
            }
            else
            {
                List<GetUserResponse> data = new List<GetUserResponse>();

                List<UserGet> unicos = users.DistinctBy(x => x.Id).ToList();

                unicos.ForEach(u =>
                {
                    data.Add(new GetUserResponse()
                    {
                        Id = u.Id,
                        NameComplete = u.NameComplete,
                        Applications = applications(users.ToList().Where(u => u.Id == u.Id).ToList())
                    });
                });

                response.Success = true;
                response.Message = Message.WHITDATA;
                response.Data = data;
            }

            return response;
        }

        private List<Application> applications(List<UserGet> applicationsUser)
        {
            List<Application> response = null;

            if (applicationsUser is not null)
            {
                response = new List<Application>();
                applicationsUser.ForEach(u =>
                {
                    response.Add(new Application { Icon = u.Icon, Name = u.Application });
                });
            }
            return response;
        }
    }
}
