using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Domain.Command.UserDomainCreate
{
    internal class UserDomainCreateHandler : IRequestHandler<UserDomainCreate, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDomainCreateHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UserDomainCreate request, CancellationToken cancellationToken)
        {
            // Validar y resolver el dominio por Code (UUID)
            if (!Guid.TryParse(request.DomainCode, out Guid domainCode))
                throw new ApiException("El código de dominio no es válido.");

            var domainList = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(
                new Dictionary<string, object> { { "Code", domainCode } },
                Schemas.Security
            );

            if (!domainList.Any())
                throw new ApiException("Dominio no encontrado.");

            int domainId = domainList.First().Id;

            // Validar que el usuario existe
            var userList = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                new Dictionary<string, object> { { "Id", request.UserId } },
                Schemas.Security
            );

            if (!userList.Any())
                throw new ApiException("Usuario no encontrado.");

            // Validar que el rol existe
            var roleList = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(
                new Dictionary<string, object> { { "Id", request.RoleId } },
                Schemas.Security
            );

            if (!roleList.Any())
                throw new ApiException("Rol no encontrado.");

            // Verificar si ya existe una relación activa
            var existingRelation = await _unitOfWork.Repository<domain.UserDomain.UserDomain>().QueryAsync<domain.UserDomain.UserDomain>(
                new Dictionary<string, object>
                {
                    { "UserId", request.UserId },
                    { "DomainId", domainId },
                    { "IsDeleted", false }
                },
                Schemas.Security
            );

            if (existingRelation.Any())
                throw new ApiException("El usuario ya está asociado a este dominio.");

            // Verificar si existe una relación eliminada para reactivarla
            var deletedRelation = await _unitOfWork.Repository<domain.UserDomain.UserDomain>().QueryAsync<domain.UserDomain.UserDomain>(
                new Dictionary<string, object>
                {
                    { "UserId", request.UserId },
                    { "DomainId", domainId },
                    { "IsDeleted", true }
                },
                Schemas.Security
            );

            if (deletedRelation.Any())
            {
                await _unitOfWork.Repository<domain.UserDomain.UserDomain>().UpdateByConditionsAsync<domain.UserDomain.UserDomain>(
                    conditions: new Dictionary<string, object>
                    {
                        { "UserId", request.UserId },
                        { "DomainId", domainId }
                    },
                    values: new Dictionary<string, object>
                    {
                        { "RoleId", request.RoleId },
                        { "IsDeleted", false },
                        { "UserUpdate", request.UserRegistration },
                        { "DateUpdate", DateTime.Now }
                    },
                    Schemas.Security
                );

                return new Response<bool>(true, "Usuario asociado al dominio exitosamente.");
            }

            // Crear la relación
            await _unitOfWork.Repository<domain.UserDomain.UserDomain>().InsertNonIdAsync(
                new domain.UserDomain.UserDomain
                {
                    UserId = request.UserId,
                    DomainId = domainId,
                    RoleId = request.RoleId,
                    UserRegistration = request.UserRegistration,
                    RegistrationDate = DateTime.Now,
                    UserUpdate = request.UserRegistration,
                    DateUpdate = DateTime.Now,
                    UserDeleted = request.UserRegistration,
                    DateDeleted = DateTime.Now
                },
                Schemas.Security
            );

            return new Response<bool>(true, "Usuario asociado al dominio exitosamente.");
        }
    }
}
