namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class TwitterUserSeeder
    {
        public static void SeedSampleTwitterUserData(TwittRDbContext context)
        {
            if (!context.TwitterUsers.Any())
            {
                context.TwitterUsers.Add(new AutoFaker<TwitterUser>());
                context.TwitterUsers.Add(new AutoFaker<TwitterUser>());
                context.TwitterUsers.Add(new AutoFaker<TwitterUser>());

                context.SaveChanges();
            }
        }
    }
}