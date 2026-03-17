using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Command.UserDomainDelete
{
    internal class UserDomainDeleteHandler : IRequestHandler<UserDomainDelete, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDomainDeleteHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserDomainDelete request, CancellationToken cancellationToken)
        {
            // Resolver dominio por Code (UUID)
            if (!Guid.TryParse(request.DomainCode, out Guid domainCode))
                throw new ApiException("El código de dominio no es válido.");

            var domainList = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                new Dictionary<string, object> { { "Code", domainCode } },
                Schemas.Security
            );

            if (!domainList.Any())
                throw new ApiException("Dominio no encontrado.");

            int domainId = domainList.First().Id;

            // Verificar que la relación existe y está activa
            var relationList = await _unitOfWork.Repository<domain.UserDomain.UserDomain>().QueryAsync<domain.UserDomain.UserDomain>(
                new Dictionary<string, object>
                {
                    { "UserId", request.UserId },
                    { "DomainId", domainId },
                    { "IsDeleted", false }
                },
                Schemas.Security
            );

            if (!relationList.Any())
                throw new ApiException("El usuario no está asociado a este dominio.");

            // Soft delete
            await _unitOfWork.Repository<domain.UserDomain.UserDomain>().UpdateByConditionsAsync<domain.UserDomain.UserDomain>(
                conditions: new Dictionary<string, object>
                {
                    { "UserId", request.UserId },
                    { "DomainId", domainId }
                },
                values: new Dictionary<string, object>
                {
                    { "IsDeleted", true },
                    { "UserDeleted", request.UserDeleted },
                    { "DateDeleted", DateTime.Now }
                },
                Schemas.Security
            );

            return new Response<bool>(true, "Usuario desasociado del dominio exitosamente.");
        }
    }
}
