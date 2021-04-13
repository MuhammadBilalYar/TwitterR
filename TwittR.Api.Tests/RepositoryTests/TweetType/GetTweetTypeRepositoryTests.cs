
namespace TwittR.Api.Tests.RepositoryTests.TweetType
{
    using Application.Dtos.TweetType;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetType;
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

    public class GetTweetTypeRepositoryTests
    { 
        
        [Fact]
        public void GetTweetType_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetType = new FakeTweetType { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetType);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                             var tweetTypeById = service.GetTweetType(fakeTweetType.TweetTypeId);
                                tweetTypeById.TweetTypeId.Should().Be(fakeTweetType.TweetTypeId);
                tweetTypeById.TweetTypeName.Should().Be(fakeTweetType.TweetTypeName);
                tweetTypeById.TweetTypeDescription.Should().Be(fakeTweetType.TweetTypeDescription);
            }
        }
        
        [Fact]
        public async void GetTweetTypesAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            var fakeTweetTypeThree = new FakeTweetType { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto());

                             tweetTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeOne);
                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeTwo);
                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetTypesAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            
                     fakeTweetTypeOne.TweetTypeId = 1;
            fakeTweetTypeTwo.TweetTypeId = 2;
            fakeTweetTypeThree.TweetTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { PageSize = 2 });

                             tweetTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeOne);
                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetTypesAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            
                     fakeTweetTypeOne.TweetTypeId = 1;
            fakeTweetTypeTwo.TweetTypeId = 2;
            fakeTweetTypeThree.TweetTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { PageSize = 1, PageNumber = 2 });

                             tweetTypeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                tweetTypeRepo.Should().ContainEquivalentOf(fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeId = 2;

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeId = 1;

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "TweetTypeId" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeTwo, fakeTweetTypeOne, fakeTweetTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeId = 2;

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeId = 1;

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "-TweetTypeId" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeThree, fakeTweetTypeOne, fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeNameSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeName = "bravo";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeName = "alpha";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "TweetTypeName" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeTwo, fakeTweetTypeOne, fakeTweetTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeNameSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeName = "bravo";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeName = "alpha";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "-TweetTypeName" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeThree, fakeTweetTypeOne, fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeDescriptionSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeDescription = "bravo";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeDescription = "alpha";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "TweetTypeDescription" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeTwo, fakeTweetTypeOne, fakeTweetTypeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_ListTweetTypeDescriptionSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeDescription = "bravo";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeDescription = "alpha";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { SortOrder = "-TweetTypeDescription" });

                             tweetTypeRepo.Should()
                    .ContainInOrder(fakeTweetTypeThree, fakeTweetTypeOne, fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTweetTypesAsync_FilterTweetTypeIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeId = 1;

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeId = 2;

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { Filters = $"TweetTypeId == {fakeTweetTypeTwo.TweetTypeId}" });

                             tweetTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_FilterTweetTypeNameListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeName = "alpha";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeName = "bravo";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { Filters = $"TweetTypeName == {fakeTweetTypeTwo.TweetTypeName}" });

                             tweetTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetTypesAsync_FilterTweetTypeDescriptionListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetTypeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            fakeTweetTypeOne.TweetTypeDescription = "alpha";

            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();
            fakeTweetTypeTwo.TweetTypeDescription = "bravo";

            var fakeTweetTypeThree = new FakeTweetType { }.Generate();
            fakeTweetTypeThree.TweetTypeDescription = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo, fakeTweetTypeThree);
                context.SaveChanges();

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));

                var tweetTypeRepo = await service.GetTweetTypesAsync(new TweetTypeParametersDto { Filters = $"TweetTypeDescription == {fakeTweetTypeTwo.TweetTypeDescription}" });

                             tweetTypeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}