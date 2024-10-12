using auth0rize.auth.domain.Primitives;

namespace auth0rize.auth.domain.Application
{
    public static class ApplicationProcedures
    {
        private static string table = "organization.application";
        public static string CREATE = $"INSERT INTO {table} ({EConsulting.parametersRows})" +
                                      $"VALUES({EConsulting.parametersValues}) RETURNING id;";
        public static string GET = $"SELECT id, name, code, description, registrationdate FROM {table} WHERE userregistration = [userId] and isdeleted = false";
        public static string GET_BYID = $"SELECT id, name, code, description, registrationdate FROM {table} WHERE id = [applicationId] and isdeleted = false";
        public static string DELETED = $"UPDATE {table} SET isDeleted = true, userdeleted = [userId] where id = [id]";
    }
}
