namespace cens.auth.infraestructure.StoredProcedures
{
    public static class UserProcedure
    {
        public static string create = "identity.User_Create";
        public static string getByKey = "identity.User_GetByKey";
        public static string get = "identity.User_Get";
        public static string update = "identity.User_Update";
        public static string delete = "identity.User_Delete";
    }
}