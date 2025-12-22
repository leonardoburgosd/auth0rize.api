using auth0rize.auth.application.Features.User.Queries.UserGet;

namespace auth0rize.auth.application.Features.User.Queries.UserGetById
{
    public class UserGetByIdResponse
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Active { get; set; }
        public int Pending { get; set; }
        public int Deleted { get; set; }
        public List<UserListResponse> Users { get; set; }
    }
}
