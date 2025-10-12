using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.VerificationUser
{
    internal class VerificationUserHandler : IRequestHandler<VerificationUser, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public VerificationUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(VerificationUser request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            var userTypes = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(new Dictionary<string, object>
            {
                { "name", "superadmin" }
            },
            Schemas.Security);
            if (userTypes.Count() == 0 || userTypes.Count() > 1) throw new ApiException("Tipo de usuario no encontrado.");

            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(new Dictionary<string, object> { { "TypeId", 2 } }, Schemas.Security);
            if (users.Count() == 0)
            {
                response.Data = false;
                response.Message = "No se encontraron registros.";
                response.Success = true;

                return response;
            }

            response.Data = true;
            response.Message = "Verificación exitosa.";
            response.Success = true;

            return response;
        }
    }
}
