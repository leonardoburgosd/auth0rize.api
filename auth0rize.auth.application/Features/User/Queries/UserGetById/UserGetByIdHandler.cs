using auth0rize.auth.application.Features.User.Queries.UserGet;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetById
{
    internal class UserGetByIdHandler : IRequestHandler<UserGetById, Response<UserGetByIdResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        public UserGetByIdHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        #endregion 

        public async Task<Response<UserGetByIdResponse>> Handle(UserGetById request, CancellationToken cancellationToken)
        {
            Response<UserGetByIdResponse> response = new Response<UserGetByIdResponse>();
            var datos = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                filters: new Dictionary<string, object>
                {
                    { "id", request.id}
                },
                schema: Schemas.Security
                );
            if (datos.Count() == 0)
            {
                response.Message = "Usuario no encontrado.";
                response.Success = false;
                return response;
            }

            response.Data = new UserGetByIdResponse()
            {
                Id = datos.First().Id,
                Email = datos.First().Email,
                LastLogin = datos.First().LastLogin.ToString(),
                FirstName = datos.First().FirstName,
                LastName = datos.First().LastName,
                MotherLastName = datos.First().MotherLastName,
                Deleted = datos.First().IsDeleted,
                TypeId = datos.First().TypeId
            };
            response.Success = true;
            response.Message = "Usuario encontrado.";

            return response;
        }
    }
}
