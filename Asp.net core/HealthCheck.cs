    //https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
    public static class HealthChecksExtension
    {
        public static void InitializeHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration["ConnectionStrings:DefaultConnection"])
                .AddRedis(RedisService.GetConfigString(configuration).ToString());

            services.AddHealthChecksUI()
            .AddInMemoryStorage();

            services
                .AddHealthChecksUI(options =>
                {
                    options.AddHealthCheckEndpoint("Healthcheck API", "/healthcheck");
                })
                .AddInMemoryStorage();
        }

        public static void UseHealthCheck(this IApplicationBuilder app)
        {
            app.UseRouting()
               .UseEndpoints(config =>
               {
                   config.MapHealthChecks("/healthcheck", new() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
                   config.MapHealthChecksUI(options => options.UIPath = "/dashboard");
               });
        }
    }