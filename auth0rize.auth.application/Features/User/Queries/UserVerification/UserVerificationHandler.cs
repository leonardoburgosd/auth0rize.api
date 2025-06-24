using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserVerification
{
    internal class UserVerificationHandler : IRequestHandler<UserVerification, Response<UserVerificationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserVerificationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserVerificationResponse>> Handle(UserVerification request, CancellationToken cancellationToken)
        {
            Response<UserVerificationResponse> response = new Response<UserVerificationResponse>();

            var users = await _unitOfWork.Repository<domain.User.User>().QueryWithRelationsAsync<domain.User.User>(
                entitySql: "SELECT * FROM security.user WHERE isDeleted = false and email = @Email",
                parameters: new { Email = request.userName },
                relationsPaths: new string[] { "UsersDomains" }
            );

            response.Success = true;
            response.Data = new UserVerificationResponse()
            {
                Email = users.First().Email,
                UserName = users.First().UserName,
            };
            response.Message = "Usuario verificado correctamente.";

            return response;
        }
    }
}
