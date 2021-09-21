using eAuction.BaseLibrary.Middleware;
using eAuction.Buyer.Contract.Commands;
using eAuction.Buyer.Contract.Query;
using eAuction.Buyer.EndPoint.Handlers;
using eAuction.Buyer.EndPoint.Saga;
using eAuction.Buyer.EndPoint.Saga.AddProduct;
using eAuction.Buyer.EndPoint.Saga.DeleteProduct;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

namespace eAuction.Buyer.EndPoint.Extensions
{
    /// <summary>
    /// MassTransitExtension
    /// </summary>
    public static class MassTransitExtension
    {
        /// <summary>
        /// AddCustomMassTransit
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,IConfiguration configuration)
        {
            var rabbitmqConfig = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            var sagaConfig = configuration.GetSection(nameof(SagaDataBaseSettings)).Get<SagaDataBaseSettings>();
            services.AddSingleton<BsonClassMap<AddProductRequestState>, AddProductRequestClassMap>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddProductCommandHandler>();
                x.AddConsumer<CreateSellerCommandHandler>();
                x.AddConsumer<GetSellerIdByEmailQueryHandler>();
                x.AddConsumer<DeleteProductCommandHandler>();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<AddProductRequestStateMachine, AddProductRequestState>()
                .MongoDbRepository(r =>
                {
                    r.Connection = sagaConfig.ConnectionString;
                    r.DatabaseName = sagaConfig.DatabaseName;
                });
                x.AddSagaStateMachine<DeleteProductRequestStateMachine, DeleteProductRequestState>()
                .MongoDbRepository(r =>
                {
                    r.Connection = sagaConfig.ConnectionString;
                    r.DatabaseName = sagaConfig.DatabaseName;
                });
                x.AddPublishMessageScheduler();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmqConfig.GetServiceUri(), h => {
                        h.Username(rabbitmqConfig.User);
                        h.Password(rabbitmqConfig.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
                
            });
            services.AddMassTransitHostedService();
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            return services;
        }



    /// <summary>
    ///  EnableCustomMassTransit
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IApplicationBuilder EnableCustomMassTransit(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            return app;
        }
    }

}
