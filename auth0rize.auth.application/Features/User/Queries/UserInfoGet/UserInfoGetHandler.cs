using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Features.Role.Queries.RoleGet;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserInfoGet
{
    internal class UserInfoGetHandler : IRequestHandler<UserInfoGet, Response<UserInfoGetResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UserInfoGetHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<UserInfoGetResponse>> Handle(UserInfoGet request, CancellationToken cancellationToken)
        {
            var userList = await _unitOfWork.Repository<auth0rize.auth.domain.User.User>().QueryAsync<auth0rize.auth.domain.User.User>(
                new Dictionary<string, object> { { "Id", request.UserId } }, 
                Schemas.Security
            );

            var user = userList.FirstOrDefault();

            if (user == null)
            {
                throw new ApiException("Usuario no encontrado.");
            }

            var typesResponse = await _mediator.Send(new TypeGet());
            var userType = typesResponse.Data?.FirstOrDefault(t => t.Id == user.TypeId)?.Name ?? "No asignado";

            var responseData = new UserInfoGetResponse
            {
                IsDoubleFactorActive = user.IsDoubleFactorActive,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MotherLastName = user.MotherLastName,
                Avatar = user.Avatar,
                UserName = user.UserName,
                Email = user.Email,
                UserType = userType
            };

            return new Response<UserInfoGetResponse>(responseData, "Información de usuario obtenida correctamente.");
        }
    }
}
