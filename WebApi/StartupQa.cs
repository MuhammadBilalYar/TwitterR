namespace WebApi
{
    using Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence;
    using Infrastructure.Shared;
    using WebApi.Extensions;
    using Serilog;

    public class StartupQa
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public StartupQa(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

                  public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsService("MyCorsPolicy");
            services.AddApplicationLayer();
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddApiVersioningExtension();
            services.AddHealthChecks();

            #region Dynamic Services
            services.AddSwaggerExtension(_config);
            #endregion
        }

             public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");
                     app.UseHsts();
            
                              app.UseHttpsRedirection();
            
            app.UseCors("MyCorsPolicy");

            app.UseSerilogRequestLogging();
            app.UseRouting();
            
            app.UseErrorHandlingMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health");
                endpoints.MapControllers();
            });

            #region Dynamic App
            app.UseSwaggerExtension(_config);
            #endregion
        }
    }
}
