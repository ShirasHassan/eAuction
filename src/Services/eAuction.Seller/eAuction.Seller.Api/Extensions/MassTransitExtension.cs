﻿using eAuction.AuctionBC.Contract.Queries;
using eAuction.BaseLibrary.Middleware;
using eAuction.Seller.Message;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eAuction.Seller.Api.Extensions
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
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var rabbitmqConfig = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(rabbitmqConfig.GetServiceUri(), h => {
                        h.Username(rabbitmqConfig.User);
                        h.Password(rabbitmqConfig.Password);
                    });
                });
                x.AddRequestClient<AddProductRequest>();
                x.AddRequestClient<ProductDeletedRequest>();
                x.AddRequestClient<GetAuctionDetailsQuery>();
                x.AddRequestClient<ListProductRequest>();
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
