using System.Reflection;

namespace auth0rize.auth.infraestructure.Extensions
{
    public class Parameters
    {
        public static string GetPropertyNamesAsString<T>()
        {
            return string.Join(",", typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Select(p => "@"+p.Name));
        }
    }
}
