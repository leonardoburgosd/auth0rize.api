using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserUpdate
{
    internal class UserUpdateHandler : IRequestHandler<UserUpdate, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserUpdateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserUpdate request, CancellationToken cancellationToken)
        {
            var userList = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                new Dictionary<string, object> { { "Id", request.UserId } },
                Schemas.Security
            );

            var user = userList.FirstOrDefault();

            if (user == null)
            {
                throw new ApiException("Usuario no encontrado.");
            }

            // Validar si el nuevo UserName o Email ya existen en otro usuario
            var existingUser = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                new Dictionary<string, object> { { "UserName", request.UserName } },
                Schemas.Security
            );
            if (existingUser.Any(u => u.Id != user.Id)) throw new ApiException("El nombre de usuario ya está en uso.");

            existingUser = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                new Dictionary<string, object> { { "Email", request.Email } },
                Schemas.Security
            );
            if (existingUser.Any(u => u.Id != user.Id)) throw new ApiException("El correo electrónico ya está en uso.");

            // Actualizar campos
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.MotherLastName = request.MotherLastName;
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.TypeId = request.TypeId;
            
            user.DateUpdate = DateTime.Now;
            user.UserUpdate = request.UserId;

            await _unitOfWork.Repository<domain.User.User>().UpdateAsync(user, Schemas.Security);

            return new Response<bool>(true, "Datos de usuario actualizados correctamente.");
        }
    }
}
