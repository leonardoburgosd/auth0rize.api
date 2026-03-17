using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Company.Command.CompanyCreate
{
    internal class CompanyCreateHandler : IRequestHandler<CompanyCreate, Response<CompanyCreateResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyCreateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<CompanyCreateResponse>> Handle(CompanyCreate request, CancellationToken cancellationToken)
        {
            // Validar formato UUID
            if (!Guid.TryParse(request.DomainId, out Guid domainCode))
            {
                throw new ApiException("El código de dominio no tiene un formato válido.");
            }

            // Buscar el dominio por código UUID
            var domain = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                new Dictionary<string, object> { { "Code", domainCode } },
                Schemas.Security
            );

            if (!domain.Any())
            {
                throw new ApiException("El dominio especificado no existe.");
            }

            var domainEntity = domain.First();

            // Crear la compañía
            int companyId = await _unitOfWork.Repository<domain.Company.Company>().InsertAsync(
                new domain.Company.Company
                {
                    Name = request.Name,
                    DomainId = domainEntity.Id,
                    Avatar = request.Avatar,
                    UserRegistration = request.UserRegistration
                },
                Schemas.Organization
            );

            return new Response<CompanyCreateResponse>(
                new CompanyCreateResponse
                {
                    Id = companyId,
                    Name = request.Name,
                    DomainCode = request.DomainId,
                    Avatar = request.Avatar
                },
                "Compañía creada exitosamente."
            );
        }
    }
}
