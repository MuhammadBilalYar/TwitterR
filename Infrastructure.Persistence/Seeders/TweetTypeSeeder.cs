namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TweetTypeSeeder
    {
        public static void SeedSampleTweetTypeData(TwittRDbContext context)
        {
            if (!context.TweetTypes.Any())
            {
                context.TweetTypes.Add(new AutoFaker<TweetType>());
                context.TweetTypes.Add(new AutoFaker<TweetType>());
                context.TweetTypes.Add(new AutoFaker<TweetType>());

                context.SaveChanges();
            }
        }
    }
}