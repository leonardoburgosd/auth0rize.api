using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace auth0rize.auth.application.Features.User.Command.UserRegister
{
    internal sealed class UserRegisterHandler : IRequestHandler<UserRegister, Response<UserRegisterResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserRegisterHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        #endregion 

        public async Task<Response<UserRegisterResponse>> Handle(UserRegister request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
