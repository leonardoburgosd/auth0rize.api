using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace cens.auth.application.Common.Security
{
    public static class Encrypt
    {
        public static bool compareHash(string password, byte[] bytePassword, byte[] salt)
        {
            using (HMACSHA512 sha512 = new HMACSHA512(salt))
            {
                byte[] hashPassword = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hashPassword.SequenceEqual(bytePassword);
            }
        }

        public static (byte[] salt, byte[] hashPassword) generateHash(string password)
        {
            using (HMACSHA512 sha512 = new HMACSHA512())
            {
                byte[] salt = sha512.Key;
                byte[] hashPassword = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (salt, hashPassword);
            }
        }

        public static string generateTokenValidation(TokenParameters tokenParams)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenParams.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.UniqueName, tokenParams.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, tokenParams.UserName),
                new Claim("UserId", tokenParams.Id.ToString()),
                new Claim("UserRol", tokenParams.Role),
                new Claim("Avatar", tokenParams.Avatar),
                new Claim("BaseCensDrive", tokenParams.Drive),
                new Claim("Email", tokenParams.Email),
                new Claim("TwoFactor",tokenParams.MultipleFactor.ToString())
            };
            var token = new JwtSecurityToken(issuer: tokenParams.Issuer,
                                             audience: tokenParams.Audience,
                                             claims: claims,
                                             expires: DateTime.Now.AddHours(tokenParams.HoursExpires),
                                             signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
