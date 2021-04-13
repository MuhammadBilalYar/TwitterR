namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class MessageSeeder
    {
        public static void SeedSampleMessageData(TwittRDbContext context)
        {
            if (!context.Messages.Any())
            {
                context.Messages.Add(new AutoFaker<Message>());
                context.Messages.Add(new AutoFaker<Message>());
                context.Messages.Add(new AutoFaker<Message>());

                context.SaveChanges();
            }
        }
    }
}