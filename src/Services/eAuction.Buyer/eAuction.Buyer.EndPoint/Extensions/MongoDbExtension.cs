using eAuction.BaseLibrary.Domain;
using eAuction.BaseLibrary.Infrastructure;
using eAuction.Buyer.Domain.BuyerAggregate;
using eAuction.Buyer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace eAuction.Buyer.EndPoint.Extensions
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
            services.AddScoped<IBuyerRepository, BuyerRepository>();
            
            return services;
        }
        
        /// <summary>
        ///  EnableCustomMongoDb
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder EnableCustomMongoDb(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            return app;
        }
    }
}