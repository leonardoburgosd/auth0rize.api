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
                }
                );
            if(datos.Count() == 0) 
            

            return response;
        }
    }
}
