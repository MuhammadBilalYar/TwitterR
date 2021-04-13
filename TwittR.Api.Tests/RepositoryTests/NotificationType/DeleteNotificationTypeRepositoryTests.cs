
namespace TwittR.Api.Tests.RepositoryTests.NotificationType
{
    using Application.Dtos.NotificationType;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.NotificationType;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;
    using Application.Interfaces;
    using Moq;

    public class DeleteNotificationTypeRepositoryTests
    { 
        
        [Fact]
        public void DeleteNotificationType_ReturnsProperCount()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteNotificationType(fakeNotificationTypeTwo);

                context.SaveChanges();

                             var notificationTypeList = context.NotificationTypes.ToList();

                notificationTypeList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                notificationTypeList.Should().ContainEquivalentOf(fakeNotificationTypeOne);
                notificationTypeList.Should().ContainEquivalentOf(fakeNotificationTypeThree);
                Assert.DoesNotContain(notificationTypeList, n => n == fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}