namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class NotificationTypeSeeder
    {
        public static void SeedSampleNotificationTypeData(TwittRDbContext context)
        {
            if (!context.NotificationTypes.Any())
            {
                context.NotificationTypes.Add(new AutoFaker<NotificationType>());
                context.NotificationTypes.Add(new AutoFaker<NotificationType>());
                context.NotificationTypes.Add(new AutoFaker<NotificationType>());

                context.SaveChanges();
            }
        }
    }
}