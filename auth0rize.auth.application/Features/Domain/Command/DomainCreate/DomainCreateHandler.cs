using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.DomainCreate
{
    internal class DomainCreateHandler : IRequestHandler<DomainCreate, Response<DomainCreateResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        public DomainCreateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<DomainCreateResponse>> Handle(DomainCreate request, CancellationToken cancellationToken)
        {
            Response<DomainCreateResponse> response = new Response<DomainCreateResponse>();

            domain.Domain.Domain domain = new domain.Domain.Domain()
            {
                UserRegistration = request.userId
            };

            int idDomain = await _unitOfWork.Repository<domain.Domain.Domain>().InsertAsync(domain, Schemas.Security);

            response.Message = "Dominio creado correctamente.";
            response.Success = true;
            response.Data = new DomainCreateResponse()
            {
                Id = idDomain,
                Code = domain.Code.ToString(),
            };

            return response;
        }
    }
}
