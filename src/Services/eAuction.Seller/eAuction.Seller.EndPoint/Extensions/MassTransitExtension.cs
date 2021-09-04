using System;
using Automatonymous;
using eAuction.Seller.Contract.Messages;
using eAuction.Seller.EndPoint.Handlers;
using eAuction.Seller.EndPoint.Saga;
using MassTransit;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace eAuction.Seller.EndPoint.Extensions
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
            services.AddSingleton<BsonClassMap<OrderState>, OrderStateClassMap>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddProductHandler>();
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitmqConfig.GetServiceUri(), h =>
                    {
                        h.Username(rabbitmqConfig.User);
                        h.Password(rabbitmqConfig.Password);
                    });
                    cfg.ReceiveEndpoint(nameof(AddProductHandler), e =>
                    {
                        e.ConfigureConsumer<AddProductHandler>(context);
                    });
                });
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .MongoDbRepository(r =>
                    {
                        r.Connection = sagaConfig.ConnectionString;
                        r.DatabaseName = sagaConfig.DatabaseName;
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
