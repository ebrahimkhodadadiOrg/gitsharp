using Cache.Redis.Entity;
using System.Linq.Expressions;

namespace Cache.Redis.Repository.Contract;

/// <summary>
/// Cache Repository
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICacheRepository<T> where T : CacheBaseEntity
{
    /// <summary>
    /// get entity by Id
    /// </summary>
    /// <param name="id">string Id</param>
    /// <returns></returns>
    Task<T> GetByIdAsync(int shopId, string id);

    /// <summary>
    /// Get All
    /// </summary>
    /// <returns>List of entities</returns>
    Task<IEnumerable<T>> GetAllAsync(int shopId);

    /// <summary>
    /// Insert entity
    /// </summary>
    /// <param name="entity">entity</param>
    /// <param name="expire">expire DateTime (when this time raised remove record)</param>
    /// <returns>entity added</returns>
    Task<T> AddAsync(int shopId, T entity, TimeSpan expire);   
    
    /// <summary>
    /// Insert list of entity
    /// </summary>
    /// <param name="entity">entity list</param>
    /// <param name="expire">expire DateTime (when this time raised remove record)</param>
    /// <returns>entity inserted list</returns>
    Task<List<T>> AddRangAsync(int shopId, List<T> entityList, TimeSpan expire);

    /// <summary>
    /// Update Entity
    /// </summary>
    /// <param name="id">entity Id</param>
    /// <param name="value">new Entity</param>
    /// <param name="expire">new expire DateTime (when this time raised remove record)</param>
    /// <returns></returns>
    Task<T> UpdateAsync(int shopId, string id, T value, TimeSpan expire);

    /// <summary>
    /// Delete entity By Id
    /// </summary>
    /// <param name="id">string Id</param>
    Task DeleteAsync(string id);

    /// <summary>
    /// single or defualt by filter and shopId
    /// </summary>
    /// <param name="expression">parameters</param>
    Task<T> SingleOrDefualt(int? shopId, Expression<Func<T, bool>> expression);   
    
    /// <summary>
    /// single or defualt by filter and shopId
    /// </summary>
    /// <param name="expression">parameters</param>
    Task<T> SingleOrDefualt(Expression<Func<T, bool>> expression);

    /// <summary>
    /// where condition
    /// </summary>
    /// <returns></returns>
    //List<KeyValuePair<string, object>> FilteryBy(Expression<Func<KeyValuePair<string, object>, bool>> predicate);
}