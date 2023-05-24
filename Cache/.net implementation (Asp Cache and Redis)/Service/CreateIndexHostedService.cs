
using Microsoft.Extensions.Hosting;
using Cache.Redis.Entity;
using Serilog;

namespace Cache.Redis.Service;

public class CreateIndexHostedService : IHostedService
{
    private readonly RedisConnectionProvider _provider;

    public CreateIndexHostedService(RedisConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entityTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(mytype => mytype.IsClass && mytype.IsSubclassOf(typeof(CacheBaseEntity)));

            IRedisConnection connection = _provider.Connection;
            foreach (Type entity in entityTypes)
            {
                await connection.CreateIndexAsync(entity);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while create redis indexes");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}