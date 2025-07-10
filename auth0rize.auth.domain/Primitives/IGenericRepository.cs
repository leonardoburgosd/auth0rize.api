namespace auth0rize.auth.domain.Primitives
{
    public interface IGenericRepository<T> where T : class
    {
        Task<(IEnumerable<T> data, int totalCount)> QueryPagedAsync<T>(Dictionary<string, object> filters = null,
                                                                       string orderBy = null,
                                                                       bool ascending = true,
                                                                       int skip = 0,
                                                                       int take = 10,
                                                                       bool includeDeleted = false,
                                                                       bool useLikeFilter = false,
                                                                       string schema = "public"
                                                                   ) where T : class, new();
        Task<IEnumerable<T>> QueryWithRelationsAsync<T>(string entitySql, object? parameters = null, params string[] relationsPaths) where T : class, new();
        Task<IEnumerable<T>> QueryAsync<T>(Dictionary<string, object> filters = null, string schema = "public") where T : class, new();
        Task<int> InsertAsync<T>(T entity, string schema = "public") where T : class, new();
        Task InsertNonIdAsync<T1>(T1 entity, string schema = "public") where T1 : class, new();
        Task<int> UpdateAsync<T>(T entity, string schema = "public") where T : class, new();
        Task DeleteHardAsync<T>(Dictionary<string, object> conditions, string schema = "public") where T : class, new();
        Task<int> DeleteAsync<T>(int id, int userId, string schema = "public") where T : class, new();
        Task<int> BulkInsertAsync<T>(IEnumerable<T> entities, string schema = "public") where T : class, new();
    }
}
