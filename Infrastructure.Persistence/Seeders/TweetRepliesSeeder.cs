namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TweetRepliesSeeder
    {
        public static void SeedSampleTweetRepliesData(TwittRDbContext context)
        {
            if (!context.TweetRepliess.Any())
            {
                context.TweetRepliess.Add(new AutoFaker<TweetReplies>());
                context.TweetRepliess.Add(new AutoFaker<TweetReplies>());
                context.TweetRepliess.Add(new AutoFaker<TweetReplies>());

                context.SaveChanges();
            }
        }
    }
}