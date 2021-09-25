using eAuction.BaseLibrary.Middleware;
using eAuction.Seller.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eAuction.Seller.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private const string ServiceRoutePrefix = "e-auction/api/v1/seller";
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers(options => {
                options.Conventions.Add(new RoutePrefixConvention(new Microsoft.AspNetCore.Mvc.RouteAttribute(ServiceRoutePrefix)));
            }).
            AddNewtonsoftJson();
            // services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            services.AddAutoMapper(typeof(Startup));
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddVersionedApiExplorer(options => {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddCustomSwagger(Configuration, typeof(Startup));
            services.AddCustomMassTransit(Configuration);
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
            app.UseRouting();
            app.UseAuthorization();
            app.UseCustomSwagger($"{ServiceRoutePrefix}");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }    

    }
}
