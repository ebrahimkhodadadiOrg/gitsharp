using Anshan.Framework.Permission;
using Framework.CommonUtility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Redis.OM.Searching;
using Cache.Redis.Entity;
using Cache.Redis.Repository.Contract;
using StackExchange.Redis;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Cache.Redis.Repository;

public class RedisRepository<T> : ICacheRepository<T> where T : CacheBaseEntity
{
    private readonly IRedisCollection<T> _redisCollection;

    private readonly IDatabase _database;
    private readonly Guid _traceId;
    private readonly string _userCode;
    private const int MaxEntityCount = 1000;
    private string EntityName => typeof(T).Name;
    private readonly ILogger<RedisRepository<T>> _Logger;

    public RedisRepository(RedisConnectionProvider provider, IDatabase database, IHttpContextAccessor httpContextAccessor
        ,ILogger<RedisRepository<T>> logger)
    {
        _database = database;
        _redisCollection = provider.RedisCollection<T>();
        Claim hashClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Hash);
        _traceId = string.IsNullOrEmpty(hashClaim?.Value) ? Guid.Empty : Guid.Parse(hashClaim.Value);
        Claim userClaim = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        _userCode = userClaim?.Value!;
        _Logger = logger;
    }

    public async Task<T> GetByIdAsync(int shopId, string id)
    {
        try
        {
            Assert.NotNull(id, nameof(id));

            return await _redisCollection.Where(x => x.ShopId == shopId).FindByIdAsync(id);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Getting {id}");
            return default;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(int shopId)
    {
        var list = await _redisCollection.Where(x => x.ShopId == shopId).ToListAsync();
        return list;
    }

    public async Task<T> AddAsync(int shopId, T entity, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(entity, nameof(entity));

            entity.Created = DateTime.Now;
            entity.ShopId = shopId;

            string id = await _redisCollection.InsertAsync(entity, expire);

            entity.CacheId = id;

            return entity;
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while adding {entity}");
            return default;
        }
    }

    public async Task<List<T>> AddRangAsync(int shopId, List<T> entityList, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(entityList, nameof(entityList));

            entityList.ForEach(x => x.Created = DateTime.Now);
            entityList.ForEach(x => x.ShopId = shopId);

            foreach (var record in entityList)
            {
                string id = await _redisCollection.InsertAsync(record, expire);
                record.CacheId = id;
            }

            return entityList;
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while adding list {entityList}");
            return default;
        }
    }

    public async Task<T> UpdateAsync(int shopId, string id, T value, TimeSpan expire)
    {
        try
        {
            Assert.NotNull(value, nameof(value));

            var entity = await _redisCollection.FindByIdAsync(id);
            if (entity is null)
                return default;

            value.Modified = DateTime.Now;
            entity.ShopId = shopId;

            await _redisCollection.UpdateAsync(value);

            return value;
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Updating {value}");
            return default;
        }
    }

    public async Task DeleteAsync(string id)
    {
        try
        {
            Assert.NotNull(id, nameof(id));

            T entity = await _redisCollection.FindByIdAsync(id);
            if (entity is null)
                return;

            await _redisCollection.DeleteAsync(entity);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while Deleting {id}");
        }
    }

    public async Task<T> SingleOrDefualt(int? shopId, Expression<Func<T, bool>> expression)
    {
        try
        {
            Assert.NotNull(expression, nameof(expression));

            var compiled = expression.Compile(); // additional condition

            return await _redisCollection.SingleOrDefaultAsync(x => compiled(x) && (shopId == null || x.ShopId == shopId));
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while find single or defualt {expression}");
            return default;
        }
    }   
    
    public async Task<T> SingleOrDefualt(Expression<Func<T, bool>> expression)
    {
        try
        {
            Assert.NotNull(expression, nameof(expression));

            var compiled = expression.Compile(); // additional condition

            return await _redisCollection.SingleOrDefaultAsync(expression);
        }
        catch (Exception e)
        {
            _Logger.LogError(e, $"Error while find single or defualt {expression}");
            return default;
        }
    }

    //public List<KeyValuePair<string, object>> FilteryBy(Expression<Func<KeyValuePair<string, object>, bool>> predicate)
    //{
    //    throw new NotImplementedException("redis FilteryBy not implemented");
    //}
}
