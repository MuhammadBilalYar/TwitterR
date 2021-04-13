namespace Infrastructure.Persistence.Contexts
{
    using Application.Interfaces;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class TwittRDbContext : DbContext
    {
        public TwittRDbContext(
            DbContextOptions<TwittRDbContext> options) : base(options) 
        {
        }

        // override the OnModelCreating method here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TweetLikes>().HasKey(t => new { t.TweetId, t.LikerTwitterUserId });
            modelBuilder.Entity<TweetReplies>().HasKey(t => new { t.TweetId, t.ReplyTweetId });
            modelBuilder.Entity<TweetRetweets>().HasKey(t => new { t.TweetId, t.TweetRetweetId });
            modelBuilder.Entity<TwitterUserFollowsTwitterUser>().HasKey(t => new { t.TwitterUserId, t.FollowedTwitterUserId});
            modelBuilder.Entity<UserBookmarksTweet>().HasKey(t => new { t.TweetId, t.TwitterUserId});
        }

        #region DbSet Region - Do Not Delete
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<TweetType> TweetTypes { get; set; }
        public DbSet<TwitterUser> TwitterUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TweetRetweets> TweetRetweetss { get; set; }
        public DbSet<TweetReplies> TweetRepliess { get; set; }
        public DbSet<TweetLikes> TweetLikess { get; set; }
        public DbSet<UserBookmarksTweet> UserBookmarksTweets { get; set; }
        public DbSet<TwitterUserFollowsTwitterUser> TwitterUserFollowsTwitterUsers { get; set; }
        #endregion
    }
}