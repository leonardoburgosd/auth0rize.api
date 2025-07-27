using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.Role.Queries.RoleGet
{
    internal sealed class TypeGetHandler : IRequestHandler<TypeGet, Response<List<TypeGetResponse>>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        public TypeGetHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<List<TypeGetResponse>>> Handle(TypeGet request, CancellationToken cancellationToken)
        {
            Response<List<TypeGetResponse>> response = new Response<List<TypeGetResponse>>();
            var types = await _unitOfWork.Repository<domain.UserType.UserType>().QueryAsync<domain.UserType.UserType>(new Dictionary<string, object>
            {
                { "isdeleted", false }
            }, schema: Schemas.Security);

            response.Success = true;
            response.Message = "Lista de tipos de usuario completa.";
            response.Data = types.Select(y => new TypeGetResponse()
            {
                Id = y.Id,
                Name = y.Name
            }).ToList();

            return response;
        }
    }
}
