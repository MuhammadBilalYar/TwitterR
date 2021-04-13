namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TweetLikesSeeder
    {
        public static void SeedSampleTweetLikesData(TwittRDbContext context)
        {
            if (!context.TweetLikess.Any())
            {
                context.TweetLikess.Add(new AutoFaker<TweetLikes>());
                context.TweetLikess.Add(new AutoFaker<TweetLikes>());
                context.TweetLikess.Add(new AutoFaker<TweetLikes>());

                context.SaveChanges();
            }
        }
    }
}