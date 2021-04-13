namespace Infrastructure.Persistence
{
    using Infrastructure.Persistence.Contexts;
    using Application.Interfaces.TwitterUserFollowsTwitterUser;
    using Application.Interfaces.UserBookmarksTweet;
    using Application.Interfaces.TweetLikes;
    using Application.Interfaces.TweetReplies;
    using Application.Interfaces.TweetRetweets;
    using Application.Interfaces.Message;
    using Application.Interfaces.TwitterUser;
    using Application.Interfaces.TweetType;
    using Application.Interfaces.NotificationType;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Sieve.Services;

    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext -- Do Not Delete            
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<TwittRDbContext>(options =>
                    options.UseInMemoryDatabase($"TwittR"));
            }
            else
            {
                services.AddDbContext<TwittRDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("TwittR"),
                        builder => builder.MigrationsAssembly(typeof(TwittRDbContext).Assembly.FullName)));
            }
            #endregion

            services.AddScoped<SieveProcessor>();

            #region Repositories -- Do Not Delete
            services.AddScoped<ITwitterUserFollowsTwitterUserRepository, TwitterUserFollowsTwitterUserRepository>();
            services.AddScoped<IUserBookmarksTweetRepository, UserBookmarksTweetRepository>();
            services.AddScoped<ITweetLikesRepository, TweetLikesRepository>();
            services.AddScoped<ITweetRepliesRepository, TweetRepliesRepository>();
            services.AddScoped<ITweetRetweetsRepository, TweetRetweetsRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ITwitterUserRepository, TwitterUserRepository>();
            services.AddScoped<ITweetTypeRepository, TweetTypeRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            #endregion
        }
    }
}
