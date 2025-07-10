using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGet
{
    internal class UserGetHandler : IRequestHandler<UserGet, Response<UserGetResponse>>
    {
        #region Inyeccion
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        public UserGetHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion 

        public async Task<Response<UserGetResponse>> Handle(UserGet request, CancellationToken cancellationToken)
        {
            Response<UserGetResponse> response = new Response<UserGetResponse>();

            var datos = await _unitOfWork.Repository<domain.User.User>().QueryPagedAsync<domain.User.User>(
                //filters: new Dictionary<string, object> { { "FirstName", "Email" } }, 
                skip: request.page - 1,
                take: request.size,
                orderBy: "FirstName",
                ascending: true,
                schema: Schemas.Security);

            List<UserListResponse> usersList = new List<UserListResponse>();
            foreach (var item in datos.data)
            {
                usersList.Add(new UserListResponse()
                {
                    Id = item.Id,
                    Name = item.FirstName + " " + item.LastName,
                    Email = item.Email,
                    Deleted = item.IsDeleted,
                    //LastLogin = item.?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never",
                    Type = item.TypeId.ToString()
                });
            }

            response.Success = true;
            response.Message = "Lista de usuarios completa.";
            response.Data = new UserGetResponse()
            {
                Total = usersList.Count(),
                Page = request.page,
                Active = datos.data.Where(u => u.IsDeleted == false).Count(),
                Pending = datos.data.Where(u=> u.IsConfirmed == false || u.IsDeleted == false).Count(),
                Confirmed = datos.data.Where(u => u.IsConfirmed == true || u.IsDeleted == false).Count(),
                Users = usersList
            };

            return response;
        }
    }
}
