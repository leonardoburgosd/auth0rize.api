namespace cens.auth.application.Common.Security
{
    public class TokenParameters
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Key { get; set; } = null!;
        public bool MultipleFactor { get; set; } = false;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int HoursExpires { get; set; } = 0!;
        public string SecretKey { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Drive { get; set; } = null!;
    }
}
