namespace auth0rize.auth.application.Features.User.Queries.UserInfoGet
{
    public class UserInfoGetResponse
    {
        public bool IsDoubleFactorActive { get; set; }
        public string FirstName { get; set; }
        public string MotherLastName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}
