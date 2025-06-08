namespace auth0rize.auth.domain.Primitives
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> QueryWithRelationsAsync<T>(string entitySql, Dictionary<string, RelationQuery> relations) where T : class, new();
        Task<IEnumerable<T>> QueryAsync<T>(Dictionary<string, object> filters = null, string schema = "public") where T : class, new();
        Task<int> InsertAsync<T>(T entity, string schema = "public") where T : class, new();
        Task<int> UpdateAsync<T>(T entity, string schema = "public") where T : class, new();
        Task<int> DeleteAsync<T>(int id, int userId, string schema = "public") where T : class, new();
        Task<int> BulkInsertAsync<T>(IEnumerable<T> entities, string schema = "public") where T : class, new();
    }
}
