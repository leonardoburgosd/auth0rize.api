using cens.auth.application.Wrappers;
using cens.auth.domain.Application.Business;
using cens.auth.domain.Primitives;
using MediatR;

namespace cens.auth.application.Features.Application.Command.CreateApplication
{

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public CreateApplicationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<bool>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            int applicationId = await _unitOfWork.Application.create(new ApplicationCreate() { Icon = request.Icon, Name = request.name, UserNameRegister = request.SecurityTokenData.UserName });

            if (applicationId == 0)
            {
                response.Success = false;
                response.Message = "Applicación no se pudo crear.";
                response.Data = false;
            }
            
            response.Success = true;
            response.Message = "Applicación creada correctamete.";
            response.Data = true;

            return response;
        }
    }
}