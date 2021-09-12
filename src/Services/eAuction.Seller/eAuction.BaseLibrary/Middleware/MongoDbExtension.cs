using eAuction.BaseLibrary.Domain;
using eAuction.BaseLibrary.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eAuction.BaseLibrary.Middleware
{
    public static class MongoDbExtension
    {
        /// <summary>
        /// AddCustomMongoDb
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomMongoDb(this IServiceCollection services,
            IConfiguration configuration)
        {
            // requires using Microsoft.Extensions.Options
            services.Configure<MongoDbSettings>(
                configuration.GetSection(nameof(MongoDbSettings)));
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped<IMongoContext, MongoContext>();
            
            return services;
        }
        
    }
}