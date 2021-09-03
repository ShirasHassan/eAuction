using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace eAuction.Seller.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {   
        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// Configuration
        /// </summary>
        private IConfiguration Configuration { get; }

        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCustomSwagger(Configuration)
                .AddCustomMassTransit(new RabbitMqConfiguration()
            {
                RabbitMqRootUri = "rabbitmq://localhost",
                Password = "guest",
                UserName = "guest"
            });


        }

        
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //  app.UseHttpsRedirection();
           
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.EnableCustomSwagger(env);
        }

       

    }


    /// <summary>
    /// MassTransitExtension
    /// </summary>
    public static class MassTransitExtension
    {
        /// <summary>
        /// AddCustomMassTransit
        /// </summary>
        /// <param name="services"></param>
        /// <param name="rabbitmqConfig"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomMassTransit(this IServiceCollection services,RabbitMqConfiguration rabbitmqConfig)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<EventConsumer>();
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) => {
                    cfg.Host(new Uri(rabbitmqConfig.RabbitMqRootUri), h => {
                        h.Username(rabbitmqConfig.UserName);
                        h.Password(rabbitmqConfig.Password);
                    });
                    cfg.ReceiveEndpoint("event-listener", e => {
                        e.ConfigureConsumer<EventConsumer>(context);
                    });
                });
                x.AddRequestClient<IValueEntered>();
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

    /// <summary>
    /// RabbitMqConfiguration
    /// </summary>
    public class RabbitMqConfiguration 
    { 
        /// <summary>
        /// RabbitMqRootUri
        /// </summary>
        public  string RabbitMqRootUri { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        public  string UserName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public  string Password { get; set; }
    }

    /// <summary>
    /// EventConsumer
    /// </summary>
    public class EventConsumer : IConsumer<IValueEntered>
    {

        readonly ILogger<EventConsumer> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consumer/Handler
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IValueEntered> context)
        {
            await context.RespondAsync<IValueProcessed>(new
            {
                Value = $"Received: {context.Message.Value}"
            });
            _logger.LogInformation("Value: {Value}", context.Message.Value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IValueProcessed
    {
        /// <summary>
        /// 
        /// </summary>
        string Value { get; }
    }

    /// <summary>
    /// ValueEntered message
    /// </summary>
    public interface IValueEntered
    {
        /// <summary>
        /// 
        /// </summary>
        string Value { get; }
    }


    /// <summary>
    /// SwaggerExtension
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// AddCustomSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seller Api", Version = "v1" });
                c.EnableAnnotations();
            });
            return services;
        }
        
        /// <summary>
        ///  EnableCustomSwagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder EnableCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seller Api"));
            return app;
        }
    }
}
