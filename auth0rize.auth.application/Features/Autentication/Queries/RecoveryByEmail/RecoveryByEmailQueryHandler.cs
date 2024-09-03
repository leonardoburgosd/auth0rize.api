using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Features.Autentication.Queries.Login;
using auth0rize.auth.application.Features.Email.Common.SendEmail;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Autentication.Queries.RecoveryByEmail
{
    internal sealed class RecoveryByEmailQueryHandler : IRequestHandler<RecoveryByEmailQuery, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public RecoveryByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(RecoveryByEmailQuery request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            bool emailExist = await _unitOfWork.User.emailExist(request.email);

            if (!emailExist)
                throw new ApiException("Email no encontrado.");

            //Enviar un correo el cual enviará el nombre de usuario 

            //var email = await Mediator.Send(new SendEmailCommon(request.email, ""));

            response.Success = true;
            response.Message = "Nombre de usuario ah sido enviado a tu correo.";
            response.Data = true;

            return response;
        }

    }
}
