using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserUpdateDoubleFactor
{
    internal class UserUpdateDoubleFactorHandler : IRequestHandler<UserUpdateDoubleFactor, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserUpdateDoubleFactorHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserUpdateDoubleFactor request, CancellationToken cancellationToken)
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

            user.IsDoubleFactorActive = request.IsActive;
            user.DateUpdate = DateTime.Now;
            user.UserUpdate = request.UserId;

            await _unitOfWork.Repository<domain.User.User>().UpdateAsync(user, Schemas.Security);

            return new Response<bool>(true, $"Doble factor {(request.IsActive ? "activado" : "desactivado")} correctamente.");
        }
    }
}
