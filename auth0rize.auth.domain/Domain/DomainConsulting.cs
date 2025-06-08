namespace auth0rize.auth.domain.Domain
{
    public static class DomainConsulting
    {
        private static string table = "security.domain";
<<<<<<< Updated upstream
        public static readonly string CREATE = $"INSERT INTO {table} ({EConsulting.parametersRows})" +
                                      $"VALUES({EConsulting.parametersValues}) RETURNING id;";
        public static readonly string EXIST = $"SELECT COUNT(id) FROM {table} WHERE Name = '[name]'";
=======
>>>>>>> Stashed changes
    }
}
