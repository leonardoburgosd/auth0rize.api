using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace auth0rize.auth.application.Common.Security
{
    public class Encrypt
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

        public static JwtSecurityToken generateTokenValidation(TokenParameters tokenParams)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenParams.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[] {
                new Claim("user_id", tokenParams.Id.ToString()),
                new Claim("domain", tokenParams.Domain),
                new Claim(JwtRegisteredClaimNames.UniqueName, tokenParams.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user_rol", tokenParams.Role),
                new Claim("avatar", tokenParams.Avatar),
                new Claim("email", tokenParams.Email),
                new Claim("two_factor",tokenParams.MultipleFactor.ToString())
            };
            JwtSecurityToken token = new JwtSecurityToken(
                                             issuer: tokenParams.Issuer,
                                             audience: tokenParams.Audience,
                                             claims: claims,
                                             expires: DateTime.Now.AddHours(tokenParams.HoursExpires),
                                             signingCredentials: credentials);
            return token;
        }
    }
}
