namespace auth0rize.auth.domain.TypeUser
{
    public static class TypeUserConsulting
    {
        private static string table = "security.type";
        public static string GET = $"SELECT Id, Name FROM {table} WHERE isdeleted = false";
    }
}
