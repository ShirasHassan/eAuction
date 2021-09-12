using eAuction.BaseLibrary.Middleware;
using eAuction.Seller.Contract.Commands;
using eAuction.Seller.Contract.Query;
using eAuction.Seller.EndPoint.Handlers;
using eAuction.Seller.EndPoint.Saga;
using eAuction.Seller.EndPoint.Saga.AddProduct;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;

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
            services.AddSingleton<BsonClassMap<AddProductRequestState>, AddProductRequestClassMap>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddProductCommandHandler>();
                x.AddConsumer<CreateSellerCommandHandler>();
                x.AddConsumer<GetSellerIdByEmailQueryHandler>();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<AddProductRequestStateMachine, AddProductRequestState>()
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
                    //cfg.ReceiveEndpoint(nameof(AddProductCommandHandler), e => e.ConfigureConsumer<AddProductCommandHandler>(context));
                    //cfg.ReceiveEndpoint(nameof(CreateSellerCommandHandler), e => e.ConfigureConsumer<CreateSellerCommandHandler>(context));
                    //cfg.ReceiveEndpoint(nameof(GetSellerIdByEmailQueryHandler), e => e.ConfigureConsumer<GetSellerIdByEmailQueryHandler>(context));
                    //cfg.ReceiveEndpoint(nameof(AddProductRequestStateMachine), e =>
                    //{
                    //    e.Durable = false;
                    //    e.ConfigureSaga<AddProductRequestState>(context);
                    //});
                    // x.AddRequestClient<GetSellerIdByEmail>();
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
