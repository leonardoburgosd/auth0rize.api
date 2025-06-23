
using auth0rize.auth.domain;
using auth0rize.auth.domain.Primitives;
using Dapper;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

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

        public async Task DeleteHardAsync<T1>(Dictionary<string, object> conditions, string schema = "public") where T1 : class, new()
        {
            var tableName = GetTableName<T1>(schema);

            var props = typeof(T1).GetProperties()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Validar que todas las claves del diccionario sean propiedades válidas
            foreach (var key in conditions.Keys)
            {
                if (!props.Contains(key))
                    throw new ArgumentException($"La columna '{key}' no existe en el tipo '{typeof(T1).Name}'.");
            }

            // Construir condiciones WHERE dinámicas
            var whereClauses = conditions.Keys.Select((key, index) => $"{key} = @param{index}").ToList();
            var sql = $"DELETE FROM {tableName} WHERE {string.Join(" AND ", whereClauses)}";

            // Crear objeto anónimo con parámetros nombrados
            var paramValues = new DynamicParameters();
            int i = 0;
            foreach (var pair in conditions)
            {
                paramValues.Add($"param{i}", pair.Value);
                i++;
            }

            await _connection.ExecuteAsync(sql, paramValues);
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

        public async Task<IEnumerable<T1>> QueryWithRelationsAsync<T1>(string entitySql, object? parameters = null, params string[] relationsPaths ) where T1 : class, new()
        {
            // 1. Traer entidades principales
            var entities = (await _connection.QueryAsync<T1>(entitySql, parameters)).ToList();
            if (!entities.Any() || relationsPaths == null || relationsPaths.Length == 0)
                return entities;

            var idProp = typeof(T1).GetProperty("Id")!;
            var entityType = typeof(T1);
            var idValues = entities.Select(e => (int)idProp.GetValue(e)!).Distinct().ToArray();

            // 2. Cargar relaciones de manera recursiva
            await LoadRelationsRecursiveAsync(entities, entityType, idValues, relationsPaths);

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

        private async Task LoadRelationsRecursiveAsync(
                                IEnumerable<object> parentEntities,
                                Type parentType,
                                int[] parentIds,
                                string[] relationPaths,
                                string currentPrefix = ""
                            )
        {
            // Agrupar paths por su primer nivel
            var grouped = relationPaths
                .Where(p => p.StartsWith(currentPrefix))
                .Select(p => p.Substring(currentPrefix.Length))
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .GroupBy(p => p.Split('.')[0]);

            foreach (var group in grouped)
            {
                string propertyName = group.Key;
                string fullPath = currentPrefix + propertyName;

                PropertyInfo? propInfo = parentType.GetProperty(propertyName);
                if (propInfo == null) continue;

                bool isCollection = IsCollectionType(propInfo.PropertyType);
                Type childType = isCollection
                    ? propInfo.PropertyType.GenericTypeArguments[0]
                    : propInfo.PropertyType;

                string tableName = GetTableNameFromType(childType); // Necesita convención o atributo [Table]
                string foreignKey = GetForeignKeyName(parentType);  // Ej: UserId si el padre es User

                // Generar SQL
                string sql = $"SELECT * FROM {tableName} WHERE {foreignKey} = ANY(@Ids::int[])";
                var relatedRaw = (await _connection.QueryAsync(sql, new { Ids = parentIds })).ToList();

                // Agrupar por ID foráneo
                var relatedGrouped = relatedRaw
                    .GroupBy(r => GetPropertyValue(r, foreignKey))
                    .ToDictionary(g => (int)g.Key, g => g.ToList());

                // Asignar al objeto padre
                foreach (var parent in parentEntities)
                {
                    var parentId = (int)parent.GetType().GetProperty("Id")!.GetValue(parent)!;
                    if (!relatedGrouped.ContainsKey(parentId)) continue;

                    var rawItems = relatedGrouped[parentId];
                    if (isCollection)
                    {
                        var typedList = Activator.CreateInstance(
                            typeof(List<>).MakeGenericType(childType)
                        ) as IList;

                        foreach (var item in rawItems)
                        {
                            var typed = MapDynamicToType(item, childType);
                            typedList!.Add(typed);
                        }

                        propInfo.SetValue(parent, typedList);
                    }
                    else
                    {
                        var typed = MapDynamicToType(rawItems.First(), childType);
                        propInfo.SetValue(parent, typed);
                    }
                }

                // Reunir hijos para procesar relaciones anidadas
                var allChildren = parentEntities
                    .SelectMany(p =>
                    {
                        var value = propInfo.GetValue(p);
                        if (value == null) return Enumerable.Empty<object>();
                        return isCollection ? (value as IEnumerable<object>)! : new[] { value };
                    })
                    .Distinct()
                    .ToList();
                var idPropertyName = GetIdPropertyName(childType);
                var childIds = allChildren
                            .Select(c => c?.GetType().GetProperty(idPropertyName)?.GetValue(c))
                            .Where(id => id != null)
                            .Select(id => Convert.ToInt32(id))
                            .Distinct()
                            .ToArray();

                // Procesar relaciones hijas recursivamente
                var nestedPaths = group
                    .Select(p => p.Contains('.') ? p.Substring(p.IndexOf('.') + 1) : null)
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .ToArray();

                if (nestedPaths.Any())
                {
                    await LoadRelationsRecursiveAsync(allChildren, childType, childIds, nestedPaths, currentPrefix + propertyName + ".");
                }
            }
        }

        private string GetIdPropertyName(Type type)
        {
            // 1. Si existe "Id"
            var idProp = type.GetProperty("Id");
            if (idProp != null) return "Id";

            // 2. Buscar una propiedad que termine en "Id" y sea del tipo primario
            var match = type.GetProperties().FirstOrDefault(p => p.Name.ToLower().EndsWith("id"));
            if (match != null) return match.Name;

            throw new Exception($"No se pudo encontrar propiedad Id en {type.Name}");
        }

        private string GetTableNameFromType(Type type)
        {
            var attr = type.GetCustomAttribute<TableAttribute>();
            if (attr != null) return attr.Name;

            // Convención fallback: "security." + nombre en minúscula
            return "security." + type.Name.ToLower();
        }

        private string GetForeignKeyName(Type parentType) => (parentType.Name + "Id").ToLower();

        private object? GetPropertyValue(object obj, string propertyName)
        {
            var dict = obj as IDictionary<string, object>;
            return dict?[propertyName];
        }
    }
}
