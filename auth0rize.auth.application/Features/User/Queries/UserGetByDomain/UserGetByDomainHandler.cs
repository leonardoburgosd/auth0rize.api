using auth0rize.auth.application.Extensions;
using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetByDomain
{
    internal class UserGetByDomainHandler : IRequestHandler<UserGetByDomain, Response<UserGetByDomainResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserGetByDomainHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserGetByDomainResponse>> Handle(UserGetByDomain request, CancellationToken cancellationToken)
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

            // Obtener relaciones activas del dominio
            var userDomains = await _unitOfWork.Repository<domain.UserDomain.UserDomain>().QueryAsync<domain.UserDomain.UserDomain>(
                new Dictionary<string, object>
                {
                    { "DomainId", domainId },
                    { "IsDeleted", false }
                },
                Schemas.Security
            );

            // Obtener roles
            var roles = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(null, Schemas.Security);

            // Obtener usuarios
            var userIds = userDomains.Select(ud => ud.UserId).ToHashSet();
            var allUsers = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(
                new Dictionary<string, object> { { "IsDeleted", false } },
                Schemas.Security
            );

            var users = allUsers
                .Where(u => userIds.Contains(u.Id) &&
                    (string.IsNullOrEmpty(request.search) ||
                     u.FirstName.Contains(request.search, StringComparison.OrdinalIgnoreCase) ||
                     u.Email.Contains(request.search, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            var userList = users.Select(u =>
            {
                var ud = userDomains.First(x => x.UserId == u.Id);
                return new UserByDomainListResponse
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    UserName = u.UserName,
                    Role = roles.FirstOrDefault(r => r.Id == ud.RoleId)?.Name ?? "Sin rol",
                    LastLogin = u.LastLogin?.ToString("dd-MM-yyyy HH:mm:ss") ?? "Nunca"
                };
            })
            .Skip((request.page - 1) * request.size)
            .Take(request.size)
            .ToList();

            return new Response<UserGetByDomainResponse>(new UserGetByDomainResponse
            {
                Total = users.Count,
                Page = request.page,
                Users = userList
            }, "Lista de usuarios del dominio.");
        }
    }
}
