using System.Text;

namespace auth0rize.auth.application.Common.Security
{
    public static class Generate
    {
        public static string generateDomainCode()
        {
            int length = 25;
            Random random = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789;[{}]:,.><_-+=(*&^%$#@!|)";
            var result = new StringBuilder(length);
            var timestamp = DateTime.UtcNow.Ticks.ToString("x");
            result.Append(timestamp);
            for (int i = timestamp.Length; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
