
using auth0rize.auth.domain;
using auth0rize.auth.domain.Primitives;
using Dapper;
using System.Data;

namespace auth0rize.auth.infraestructure.Persistence
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConnection _connection;

        public GenericRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> BulkInsertAsync<T1>(IEnumerable<T1> entities, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T1>(schema);

            var props = typeof(T1).GetProperties()
                .Where(p => !p.GetMethod.IsVirtual)
                .Where(p => p.DeclaringType != typeof(BaseEntity))
                .Where(p => !IsComplexType(p.PropertyType))
                .ToList();

            var columns = string.Join(", ", props.Select(p => p.Name));
            var parameters = string.Join(", ", props.Select(p => "@" + p.Name));
            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

            return await _connection.ExecuteAsync(sql, entities);
        }

        public async Task<int> DeleteAsync<T1>(int id, int userId, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T>(schema);
            string sql;
            sql = $"UPDATE {tableName} SET IsDeleted = true, DateDeleted = NOW(), UserDeleted = @UserId WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
        }

        public async Task InsertNonIdAsync<T1>(T1 entity, string schema = "public") where T1 : class, new()
        {
            var type = typeof(T1);
            var tableName = GetTableName<T1>(schema);

            var props = type.GetProperties()
                .Where(p => !p.GetMethod.IsVirtual)
                .Where(p =>
                {
                    if (typeof(BaseEntity).IsAssignableFrom(type) && type.GetProperty("UserRegistration") != null)
                    {
                        return p.DeclaringType != typeof(BaseEntity) || p.Name == "UserRegistration";
                    }
                    else
                    {
                        return p.DeclaringType != typeof(BaseEntity);
                    }
                })
                .Where(p => p.Name != "Id")
                .ToList();

            var columns = string.Join(", ", props.Select(p => p.Name));
            var parameters = string.Join(", ", props.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

            await _connection.ExecuteAsync(sql, entity);
        }


        public async Task<int> InsertAsync<T1>(T1 entity, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T>(schema);

            var props = typeof(T).GetProperties()
                .Where(p => !p.GetMethod.IsVirtual)
                .Where(p =>
                {
                    if (typeof(BaseEntity).IsAssignableFrom(typeof(T)) && typeof(T).GetProperty("UserRegistration") != null)
                    {
                        return p.DeclaringType != typeof(BaseEntity) || p.Name == "UserRegistration";
                    }
                    else
                    {
                        return p.DeclaringType != typeof(BaseEntity); // O simplemente true si no te importa BaseEntity
                    }
                })
                .Where(p => p.Name != "Id")
                .ToList();

            var columns = string.Join(", ", props.Select(p => p.Name));
            var parameters = string.Join(", ", props.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters}) RETURNING Id";

            return await _connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<IEnumerable<T1>> QueryAsync<T1>(Dictionary<string, object> filters = null, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T1>(schema);

            string whereClause = "";
            DynamicParameters parameters = new DynamicParameters();

            if (filters != null && filters.Count > 0)
            {
                var clauses = filters.Select(f => $"{f.Key} = @{f.Key}");
                whereClause = "WHERE " + string.Join(" AND ", clauses);

                foreach (var f in filters)
                {
                    parameters.Add(f.Key, f.Value);
                }
            }

            var sql = $"SELECT * FROM {tableName} {whereClause}";
            return await _connection.QueryAsync<T1>(sql, parameters);
        }

        public async Task<IEnumerable<T1>> QueryWithRelationsAsync<T1>(string entitySql, Dictionary<string, RelationQuery> relations) where T1 : class, new()
        {
            // 1. Traer entidades principales
            var entities = (await _connection.QueryAsync<T1>(entitySql)).ToList();

            if (!entities.Any() || relations == null || !relations.Any())
                return entities;

            // 2. Obtener los Ids
            var idProp = typeof(T1).GetProperty("Id");
            var ids = entities.Select(e => (int)idProp.GetValue(e)).Distinct().ToArray();

            // 3. Procesar relaciones (1-1 y 1-N)
            foreach (var relation in relations)
            {
                var propInfo = typeof(T1).GetProperty(relation.Key);
                bool isCollection = IsCollectionType(propInfo.PropertyType);

                // Ejecutar SQL con IDs
                var relatedData = (await _connection.QueryAsync(
                    relation.Value.Sql, new { Ids = ids }
                )).ToList();

                if (isCollection)
                {
                    // 1-N: Asignar colección
                    foreach (var entity in entities)
                    {
                        var entityId = (int)idProp.GetValue(entity);

                        var relatedItems = relatedData
                            .Where(r => MatchRelationKey(r, relation.Value.ForeignKey, entityId))
                            .Select(r => MapDynamicToType(r, propInfo.PropertyType.GenericTypeArguments[0]))
                            .ToList();

                        var typedList = Activator.CreateInstance(
                            typeof(List<>).MakeGenericType(propInfo.PropertyType.GenericTypeArguments[0]),
                            relatedItems
                        );

                        propInfo.SetValue(entity, typedList);
                    }
                }
                else
                {
                    // 1-1: Asignar objeto único
                    foreach (var entity in entities)
                    {
                        var entityId = (int)idProp.GetValue(entity);

                        var relatedItem = relatedData
                            .FirstOrDefault(r => MatchRelationKey(r, relation.Value.ForeignKey, entityId));

                        if (relatedItem != null)
                        {
                            var mapped = MapDynamicToType(relatedItem, propInfo.PropertyType);
                            propInfo.SetValue(entity, mapped);
                        }
                    }
                }
            }

            return entities;
        }

        public async Task<int> UpdateAsync<T1>(T1 entity, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T>(schema);

            var props = typeof(T).GetProperties()
                .Where(p => !p.GetMethod.IsVirtual)
                .Where(p => p.DeclaringType != typeof(BaseEntity))
                .Where(p => !IsComplexType(p.PropertyType))
                .ToList();

            var setClause = string.Join(", ", props.Select(p => $"{p.Name} = @{p.Name}"));
            var sql = $"UPDATE {tableName} SET {setClause} WHERE Id = @Id";

            return await _connection.ExecuteAsync(sql, entity);
        }

        #region Usado en QueryWithRelations
        private bool MatchRelationKey(dynamic r, string foreignKey, int entityId)
        {
            var dict = (IDictionary<string, object>)r;
            return dict.ContainsKey(foreignKey.ToLower()) && (int)dict[foreignKey.ToLower()] == entityId;
        }

        private bool IsCollectionType(Type type)
        {
            return type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()) ||
                   type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        private object MapDynamicToType(dynamic source, Type targetType)
        {
            var obj = Activator.CreateInstance(targetType);
            var dict = (IDictionary<string, object>)source;

            foreach (var prop in targetType.GetProperties())
            {
                var propNameLower = prop.Name.ToLower();
                if (dict.ContainsKey(propNameLower) && dict[propNameLower] != null && dict[propNameLower] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(dict[propNameLower], prop.PropertyType));
                }
            }

            return obj;
        }
        #endregion

        #region Usado en Insert
        private string GetTableName<T>(string schema)
        {
            var name = typeof(T).Name.ToLower();

            return string.IsNullOrEmpty(schema) ? name : $"{schema}.{name}";
        }

        private bool IsComplexType(Type type)
        {
            return type.IsClass && type != typeof(string);
        }
        #endregion
    }
}
