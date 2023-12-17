using cens.auth.application.Wrappers;
using cens.auth.domain.Application.Business;
using cens.auth.domain.Primitives;
using cens.auth.drive.Entities;
using cens.auth.drive.Intefaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace cens.auth.application.Features.Application.Command.CreateApplication
{

    public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Response<bool>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDriveRepository _drive;
        private string user = "";
        private string password = "";
        public CreateApplicationCommandHandler(IUnitOfWork unitOfWork, IDriveRepository drive, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _drive = drive;
            user = Environment.GetEnvironmentVariable(configuration["drive:user"]!.ToString());
            password = Environment.GetEnvironmentVariable(configuration["drive:password"]!.ToString());

        }
        #endregion

        public async Task<Response<bool>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            Response<bool> response = new Response<bool>();

            LoginResponse auth = await _drive.auth(user, password);

            ResponseDrive<UpdateFileResponse> baseFileIcon = await _drive.uploadFile(request.Icon, auth.Token);

            int applicationId = await _unitOfWork.Application.create(new ApplicationCreate()
            {
                Icon = baseFileIcon.Data.BaseCodeFile,
                Name = request.name,
                Description = request.Description,
                RegistrationUser = request.SecurityTokenData.UserName
            });

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