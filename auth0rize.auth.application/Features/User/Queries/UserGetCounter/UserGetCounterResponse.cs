namespace auth0rize.auth.application.Features.User.Queries.UserGetCounter
{
    public class UserGetCounterResponse
    {
        public int TotalUsers { get; set; }
        public int TotalDomains { get; set; }
        public int TotalLoginSuccess { get; set; }
        public int TotalLoginFailed { get; set; }
        public List<SessionHistoryResponse> SessionHistory { get; set; }
        public List<UserRegisterHistoryResponse> UserRegisterHistory { get; set; }
        public List<LastHistoryResponse> LastHistoryResponse { get; set; }
    }

    public class LastHistoryResponse
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
    public class SessionHistoryResponse
    {
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }

    public class UserRegisterHistoryResponse
    {
        public int Count { get; set; }
        public DateTime Date { get; set; }
    }
}
