
namespace TwittR.Api.Tests
{
    using Infrastructure.Persistence.Contexts;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using WebApi;

    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
             private static Checkpoint checkpoint = new Checkpoint
        {
            
        };

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTesting");

            builder.ConfigureServices(async services =>
            {
                             var provider = services.BuildServiceProvider();

                                          services.AddDbContext<TwittRDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                             var sp = services.BuildServiceProvider();

                                          using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<TwittRDbContext>();

                                     db.Database.EnsureCreated();

                    try
                    {
                        await checkpoint.Reset(db.Database.GetDbConnection());
                    }
                    catch
                    {
                    }
                }
            });
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}