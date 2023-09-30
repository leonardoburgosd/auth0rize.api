namespace cens.auth.application.Common.Security
{
    public class TokenParameters
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Key { get; set; }
        public bool MultipleFactor { get; set; } = false;
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int HoursExpires { get; set; }
        public string SecretKey { get; set; }
        public string Avatar { get; set; }
        public string Drive { get; set; }
    }
}
