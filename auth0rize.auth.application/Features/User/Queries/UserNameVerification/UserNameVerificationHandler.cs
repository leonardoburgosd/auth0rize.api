using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserNameVerification
{
    internal sealed class UserNameVerificationHandler : IRequestHandler<UserNameVerification, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public UserNameVerificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<bool>> Handle(UserNameVerification request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();
            var datos = await _unitOfWork.Repository<domain.User.User>().QueryPagedAsync<domain.User.User>(
                filters: new Dictionary<string, object> {
                    { "UserName", request.userName },
                },
                schema: Schemas.Security);
            if (datos.data.Count() > 0)
            {
                response.Data = false;
                response.Message = "Nombre de usuario encontrado.";
                response.Success = false;
            }

            response.Data = true;
            response.Message = "Nombre de usuario no encontrado.";
            response.Success = true;

            return response;
        }
    }
}
