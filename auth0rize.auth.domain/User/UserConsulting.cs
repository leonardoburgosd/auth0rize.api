﻿using auth0rize.auth.domain.Primitives;

namespace auth0rize.auth.domain.User
{
    public static class UserConsulting
    {
        private readonly static string table = "security.user";

        public readonly static string CREATE = $"INSERT INTO {table} ({EConsulting.parametersRows})" +
                                      $"VALUES({EConsulting.parametersValues}) RETURNING id;";

        public readonly static string GET = $"SELECT {EConsulting.parametersRows} " +
                                   $"FROM {table} WHERE isdeleted = 0";

        public readonly static string GET_BY_USERNAME = $"SELECT u.id, u.name, u.lastname, u.motherlastname, " +
                                                    $"u.username, u.email, u.password, u.salt, u.isdoublefactoractivate, u.avatar, u.type as typeUser, " +
                                                    $"t.name as typeusername, u.domain, d.code as domainname " +
                                                    $"FROM {table} u " +
                                                    $"INNER JOIN security.type t on u.type = t.id " +
                                                    $"INNER JOIN security.domain d on u.domain = d.id " +
                                                    $"WHERE u.userName = [username]";
        public readonly static string COUNT_BY_USERNAME = $"SELECT COUNT(id) FROM {table} WHERE userName = '[username]'";
        public readonly static string COUNT_BY_EMAIL = $"SELECT COUNT(id) FROM {table} WHERE email = '[email]'";
        public readonly static string COUNT_BY_NUMBER = $"SELEC COUNT(id) FROM {table}";
    }
}
