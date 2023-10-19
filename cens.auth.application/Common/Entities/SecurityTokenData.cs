namespace cens.auth.application.Common.Entities
{
    public class SecurityTokenData
    {
        public int UserId { get; set; } = 0!;
        public string UserName { get; set; } = null!;
        public string TypeUser { get; set; } = null!;
    }
}
