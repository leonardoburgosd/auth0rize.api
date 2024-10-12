using auth0rize.auth.domain.Primitives;

namespace auth0rize.auth.domain.Domain
{
    public static class DomainConsulting
    {
        private static string table = "security.domain";
        public static readonly string CREATE = $"INSERT INTO {table} ({EConsulting.parametersRows})" +
                                      $"VALUES({EConsulting.parametersValues}) RETURNING id;";
        public static readonly string EXIST = $"SELECT COUNT(id) FROM {table} WHERE Name = '[name]'";
        public static readonly string GET_ID = $"SELECT d.id, d.name, d.code, d.registrationdate, true AS default FROM security.domain d " +
                                               $"WHERE d.code = '[domainCode]' UNION  SELECT d.id, d.name, d.code, d.registrationdate, " +
                                               $"false AS default FROM security.domain d WHERE isdeleted = false and userregistration = (select d.userregistration " +
                                               $"FROM security.domain d WHERE d.code = '[domainCode]' LIMIT 1) " +
                                               $"and id != (select d.id FROM security.domain d WHERE d.code = '[domainCode]' LIMIT 1)";
        public static readonly string DELETE = $"UPDATE security.domain set isdeleted = true, userdeleted = '[userId]' WHERE id = '[domainId]'";
    }
}
