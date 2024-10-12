using auth0rize.auth.application.Common.Security;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Domain.Business;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.CreateDomain
{
    internal sealed class CreateDomainHandler : IRequestHandler<CreateDomain, Response<CreateDomainResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public CreateDomainHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public async Task<Response<CreateDomainResponse>> Handle(CreateDomain request, CancellationToken cancellationToken)
        {
            Response<CreateDomainResponse> response = new Response<CreateDomainResponse>();
            string generateCode = Generate.generateDomainCode();
            long? id = await _unitOfWork.Domain.create(new DomainCreate()
            {
                Name = request.name,
                UserRegistration = request.session.Id,
                Code = generateCode
            });

            response.Success = true;
            response.Message = "Dominio creado correctamente.";
            response.Data = new CreateDomainResponse()
            {
                Code = generateCode,
                Id = id ?? 0
            };

            return response;
        }
    }
}
