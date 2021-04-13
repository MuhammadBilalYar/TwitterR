namespace WebApi
{
    using Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Infrastructure.Persistence;
    using Infrastructure.Shared;
    using Infrastructure.Persistence.Seeders;
    using Infrastructure.Persistence.Contexts;
    using WebApi.Extensions;
    using Serilog;

    public class StartupDevelopment
    {
        public IConfiguration _config { get; }
        public IWebHostEnvironment _env { get; }

        public StartupDevelopment(IConfiguration configuration, IWebHostEnvironment env)
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
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            #region Entity Context Region - Do Not Delete

                using (var context = app.ApplicationServices.GetService<TwittRDbContext>())
                {
                    context.Database.EnsureCreated();

                    #region TwittRDbContext Seeder Region - Do Not Delete
                    
                    NotificationTypeSeeder.SeedSampleNotificationTypeData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TweetTypeSeeder.SeedSampleTweetTypeData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TwitterUserSeeder.SeedSampleTwitterUserData(app.ApplicationServices.GetService<TwittRDbContext>());
                    MessageSeeder.SeedSampleMessageData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TweetRetweetsSeeder.SeedSampleTweetRetweetsData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TweetRepliesSeeder.SeedSampleTweetRepliesData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TweetLikesSeeder.SeedSampleTweetLikesData(app.ApplicationServices.GetService<TwittRDbContext>());
                    UserBookmarksTweetSeeder.SeedSampleUserBookmarksTweetData(app.ApplicationServices.GetService<TwittRDbContext>());
                    TwitterUserFollowsTwitterUserSeeder.SeedSampleTwitterUserFollowsTwitterUserData(app.ApplicationServices.GetService<TwittRDbContext>());
                    #endregion
                }

            #endregion

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
