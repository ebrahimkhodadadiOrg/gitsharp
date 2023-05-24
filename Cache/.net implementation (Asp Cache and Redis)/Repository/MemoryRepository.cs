using Anshan.Framework.Permission;
using Framework.CommonUtility;
using Microsoft.Extensions.Logging;
using Cache.Redis.Entity;
using Cache.Redis.Repository.Contract;
using System.Linq.Expressions;
using System.Runtime.Caching;

namespace Cache.Redis.Repository;

public class CacheRepository<T> : ICacheRepository<T> where T : CacheBaseEntity
{
    ObjectCache _memoryCache = MemoryCache.Default;
    private readonly Microsoft.Extensions.Caching.Memory.IMemoryCache _MemoryCache;

    private readonly ILogger<CacheRepository<T>> _Logger;

    public CacheRepository(ILogger<CacheRepository<T>> logger, Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
    {
        _Logger = logger;
        _MemoryCache = memoryCache;
    }

    public Task<T> AddAsync(int shopId, T entity, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(entity, nameof(entity));

            entity.CacheId = Guid.NewGuid().ToString().Replace("-", string.Empty);
            entity.Created = DateTime.Now;

            entity.ShopId = shopId;

            _memoryCache.Set($"{typeof(T).Name}:{entity.CacheId}", entity, new DateTimeOffset(DateTime.Now, expire));

            return Task.FromResult(entity);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while adding {entity}");
            return default;
        }
    }

    public Task<List<T>> AddRangAsync(int shopId, List<T> entityList, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(entityList, nameof(entityList));

            entityList.ForEach(x => x.CacheId = Guid.NewGuid().ToString().Replace("-", string.Empty));
            entityList.ForEach(x => x.Created = DateTime.Now);

            entityList.ForEach(x => x.ShopId = shopId);

            entityList.ForEach(x => _memoryCache.Set($"{typeof(T).Name}:{x.CacheId}", x, new DateTimeOffset(DateTime.Now, expire)));

            return Task.FromResult(entityList);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while adding list {entityList}");
            return default;
        }
    }

    public Task<T> UpdateAsync(int shopId, string id, T value, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(value, nameof(value));
            if (!_MemoryCache.TryGetValue($"{typeof(T).Name}:{id}", out object values))
                Assert.NotNull(id, nameof(id));

            value.Modified = DateTime.Now;

            value.ShopId = shopId;

            _memoryCache.Set($"{typeof(T).Name}:{id}", value, new DateTimeOffset(DateTime.Now, expire));

            return Task.FromResult(value);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Updating {value}");
            return default;
        }
    }

    public Task DeleteAsync(string id)
    {
        try
        {
            Assert.NotNull(id, nameof(id));
            if (!_MemoryCache.TryGetValue($"{typeof(T).Name}:{id}", out object values))
                Assert.NotNull(id, nameof(id));

            _memoryCache.Remove($"{typeof(T).Name}:{id}");
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Deleting {id}");
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<T>> GetAllAsync(int shopId)
    {
        var result = _memoryCache.AsQueryable()
            .Where(x => x.Key.Contains(typeof(T).Name))
            .Select(x => (T)x.Value)
            .Where(x => x.ShopId == shopId)
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<T> GetByIdAsync(int shopId, string id)
    {
        try
        {
            Assert.NotNull(id, nameof(id));

            var result = (T)_memoryCache.Get($"{typeof(T).Name}:{id}");
            return Task.FromResult(result);

        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Getting {id}");
            return default;
        }
    }

    public Task<T> SingleOrDefualt(int? shopId, Expression<Func<T, bool>> expression)
    {
        try
        {
            Assert.NotNull(expression, nameof(expression));

            var compiled = expression.Compile(); // additional condition

            var result = _memoryCache.AsQueryable()
                .Where(x => x.Key.Contains(typeof(T).Name))
                .Select(x => (T)x.Value)
                .SingleOrDefault(x => compiled(x) && (shopId == null || x.ShopId == shopId));

            return Task.FromResult(result);

        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while find single or defualt {expression}");
            return default;
        }
    }  
    
    public Task<T> SingleOrDefualt(Expression<Func<T, bool>> expression)
    {
        try
        {
            Assert.NotNull(expression, nameof(expression));

            var compiled = expression.Compile(); // additional condition

            var result = _memoryCache.AsQueryable()
                .Where(x => x.Key.Contains(typeof(T).Name))
                .Select(x => (T)x.Value)
                .SingleOrDefault(expression);

            return Task.FromResult(result);

        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while find single or defualt {expression}");
            return default;
        }
    }

    //public List<KeyValuePair<string, object>> FilteryBy(Expression<Func<KeyValuePair<string, object>, bool>> predicate)
    //{
    //    var exp = predicate.Compile();

    //    return _memoryCache.AsQueryable().Where(x => x.Key.Contains(typeof(T).Name) && exp(x)).ToList();
    //}
}