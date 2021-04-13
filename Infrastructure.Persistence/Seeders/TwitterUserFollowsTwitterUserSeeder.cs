namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TwitterUserFollowsTwitterUserSeeder
    {
        public static void SeedSampleTwitterUserFollowsTwitterUserData(TwittRDbContext context)
        {
            if (!context.TwitterUserFollowsTwitterUsers.Any())
            {
                context.TwitterUserFollowsTwitterUsers.Add(new AutoFaker<TwitterUserFollowsTwitterUser>());
                context.TwitterUserFollowsTwitterUsers.Add(new AutoFaker<TwitterUserFollowsTwitterUser>());
                context.TwitterUserFollowsTwitterUsers.Add(new AutoFaker<TwitterUserFollowsTwitterUser>());

                context.SaveChanges();
            }
        }
    }
}