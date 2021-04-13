
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

    public class GetNotificationTypeRepositoryTests
    { 
        
        [Fact]
        public void GetNotificationType_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationType = new FakeNotificationType { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationType);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                             var notificationTypeById = service.GetNotificationType(fakeNotificationType.NotificationTypeId);
                                notificationTypeById.NotificationTypeId.Should().Be(fakeNotificationType.NotificationTypeId);
                notificationTypeById.NotificationTypeTitle.Should().Be(fakeNotificationType.NotificationTypeTitle);
                notificationTypeById.NotificationTypeDescription.Should().Be(fakeNotificationType.NotificationTypeDescription);
            }
        }
        
        [Fact]
        public async void GetNotificationTypesAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto());

                             notificationTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeOne);
                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeTwo);
                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetNotificationTypesAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            
                     fakeNotificationTypeOne.NotificationTypeId = 1;
            fakeNotificationTypeTwo.NotificationTypeId = 2;
            fakeNotificationTypeThree.NotificationTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { PageSize = 2 });

                             notificationTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeOne);
                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetNotificationTypesAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            
                     fakeNotificationTypeOne.NotificationTypeId = 1;
            fakeNotificationTypeTwo.NotificationTypeId = 2;
            fakeNotificationTypeThree.NotificationTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { PageSize = 1, PageNumber = 2 });

                             notificationTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                notificationTypeRepo.Should().ContainEquivalentOf(fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeId = 2;

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeId = 1;

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "NotificationTypeId" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeTwo, fakeNotificationTypeOne, fakeNotificationTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeId = 2;

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeId = 1;

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "-NotificationTypeId" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeThree, fakeNotificationTypeOne, fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeTitleSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeTitle = "bravo";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeTitle = "alpha";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeTitle = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "NotificationTypeTitle" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeTwo, fakeNotificationTypeOne, fakeNotificationTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeTitleSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeTitle = "bravo";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeTitle = "alpha";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeTitle = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "-NotificationTypeTitle" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeThree, fakeNotificationTypeOne, fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeDescriptionSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeDescription = "bravo";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeDescription = "alpha";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "NotificationTypeDescription" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeTwo, fakeNotificationTypeOne, fakeNotificationTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_ListNotificationTypeDescriptionSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeDescription = "bravo";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeDescription = "alpha";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { SortOrder = "-NotificationTypeDescription" });

                             notificationTypeRepo.Should()
                    .ContainInOrder(fakeNotificationTypeThree, fakeNotificationTypeOne, fakeNotificationTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetNotificationTypesAsync_FilterNotificationTypeIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeId = 1;

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeId = 2;

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { Filters = $"NotificationTypeId == {fakeNotificationTypeTwo.NotificationTypeId}" });

                             notificationTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_FilterNotificationTypeTitleListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeTitle = "alpha";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeTitle = "bravo";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeTitle = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { Filters = $"NotificationTypeTitle == {fakeNotificationTypeTwo.NotificationTypeTitle}" });

                             notificationTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetNotificationTypesAsync_FilterNotificationTypeDescriptionListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"NotificationTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            fakeNotificationTypeOne.NotificationTypeDescription = "alpha";

            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();
            fakeNotificationTypeTwo.NotificationTypeDescription = "bravo";

            var fakeNotificationTypeThree = new FakeNotificationType { }.Generate();
            fakeNotificationTypeThree.NotificationTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo, fakeNotificationTypeThree);
                context.SaveChanges();

                var service = new NotificationTypeRepository(context, new SieveProcessor(sieveOptions));

                var notificationTypeRepo = await service.GetNotificationTypesAsync(new NotificationTypeParametersDto { Filters = $"NotificationTypeDescription == {fakeNotificationTypeTwo.NotificationTypeDescription}" });

                             notificationTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}