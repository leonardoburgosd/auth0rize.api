using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using auth0rize.auth.domain.User.Business;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.AllUsersQuery
{
    internal sealed class AllUsersQueryHandler : IRequestHandler<AllUsersQuery, Response<List<AllUsersQueryResponse>>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;

        public AllUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<List<AllUsersQueryResponse>>> Handle(AllUsersQuery request, CancellationToken cancellationToken)
        {
            Response<List<AllUsersQueryResponse>> response = new Response<List<AllUsersQueryResponse>>();

            IEnumerable<UserDetail> users = await _unitOfWork.User.get(request.session.Id);

            if (users is null) throw new KeyNotFoundException("No se encontraron datos de usuarios.");


            List<AllUsersQueryResponse> data = new List<AllUsersQueryResponse>();
            data = users.Select(u => new AllUsersQueryResponse()
            {
                Id = u.Id,
                Application = u.Application,
                ApplicationName = u.ApplicationName,
                Avatar = u.Avatar,
                Domain = u.Domain,
                DomainName = u.DomainName,
                Email = u.Email,
                IsDoubleFactorActivate = u.IsDoubleFactorActivate,
                LastName = u.LastName,
                MotherLastName = u.MotherLastName,
                Name = u.Name,
                TypeUser = u.TypeUser,
                TypeUserName = u.TypeUserName,
                UserName = u.UserName
            }).ToList();

            response.Success = true;
            response.Message = "Se encontraron datos de usuarios.";
            response.Data = data;

            return response;
        }
    }
}
