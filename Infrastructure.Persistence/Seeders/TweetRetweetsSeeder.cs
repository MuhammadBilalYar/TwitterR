namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TweetRetweetsSeeder
    {
        public static void SeedSampleTweetRetweetsData(TwittRDbContext context)
        {
            if (!context.TweetRetweetss.Any())
            {
                context.TweetRetweetss.Add(new AutoFaker<TweetRetweets>());
                context.TweetRetweetss.Add(new AutoFaker<TweetRetweets>());
                context.TweetRetweetss.Add(new AutoFaker<TweetRetweets>());

                context.SaveChanges();
            }
        }
    }
}