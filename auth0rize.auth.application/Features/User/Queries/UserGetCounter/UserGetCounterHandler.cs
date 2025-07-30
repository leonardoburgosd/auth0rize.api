using auth0rize.auth.application.Wrappers;
using auth0rize.auth.domain.Primitives;
using MediatR;

namespace auth0rize.auth.application.Features.User.Queries.UserGetCounter
{
    internal class UserGetCounterHandler : IRequestHandler<UserGetCounter, Response<UserGetCounterResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserGetCounterHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<UserGetCounterResponse>> Handle(UserGetCounter request, CancellationToken cancellationToken)
        {
            Response<UserGetCounterResponse> response = new Response<UserGetCounterResponse>();

            var historyLogin = await _unitOfWork.Repository<domain.Login.Login>().QueryAsync<domain.Login.Login>(schema: Schemas.History);
            var domains = await _unitOfWork.Repository<domain.Domain.Domain>().QueryAsync<domain.Domain.Domain>(new Dictionary<string, object> { { "isdeleted", false } }, Schemas.Security);
            var users = await _unitOfWork.Repository<domain.User.User>().QueryAsync<domain.User.User>(new Dictionary<string, object> { { "isdeleted", false } }, Schemas.Security);

            List<LastHistoryResponse> lastHistory = new List<LastHistoryResponse>();
            foreach (var item in historyLogin.Where(hl => hl.Checked == true).OrderByDescending(hl => hl.RegistrationDate).Take(5))
            {
                lastHistory.Add(new LastHistoryResponse()
                {
                    UserName = item.UserName,
                    Date = item.RegistrationDate
                });
            }

            response.Success = true;
            response.Message = "Conteo de usuarios completa.";
            response.Data = new UserGetCounterResponse()
            {
                TotalUsers = users.Count(),
                TotalDomains = domains.Count(),
                TotalLoginSuccess = historyLogin.Where(hl => hl.Checked == true).Count(),
                TotalLoginFailed = historyLogin.Where(hl => hl.Checked == false).Count(),
                SessionHistory = new List<SessionHistoryResponse>(),
                UserRegisterHistory = new List<UserRegisterHistoryResponse>(),
                LastHistoryResponse = lastHistory
            };

            return response;
        }
    }
}
