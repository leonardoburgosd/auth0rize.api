using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Application.Business;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Application.Common.CreateApplication
{
    internal sealed class CreateApplicationHandler : IRequestHandler<CreateApplication, Response<CreateApplicationResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public CreateApplicationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<CreateApplicationResponse>> Handle(CreateApplication request, CancellationToken cancellationToken)
        {
            Response<CreateApplicationResponse> response = new Response<CreateApplicationResponse>();

            Guid codeGenerate = GenerateCode();
            long? applicationId = await _unitOfWork.Application.create(new ApplicationCreate()
            {
                Name = request.name,
                Code = codeGenerate,
                Description = request.description,
                Userregistration = request.session.Id
            });

            if (applicationId is null) throw new ApiException("No se pudo agregar la aplicación.");

            response.Success = true;
            response.Message = "";
            response.Data = new CreateApplicationResponse()
            {
                Id = applicationId ?? 0,
                Code = codeGenerate
            };

            return response;
        }

        private Guid GenerateCode()
        {
            return Guid.NewGuid();
        }
    }
}
