namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class UserBookmarksTweetSeeder
    {
        public static void SeedSampleUserBookmarksTweetData(TwittRDbContext context)
        {
            if (!context.UserBookmarksTweets.Any())
            {
                context.UserBookmarksTweets.Add(new AutoFaker<UserBookmarksTweet>());
                context.UserBookmarksTweets.Add(new AutoFaker<UserBookmarksTweet>());
                context.UserBookmarksTweets.Add(new AutoFaker<UserBookmarksTweet>());

                context.SaveChanges();
            }
        }
    }
}