
namespace TwittR.Api.Tests.RepositoryTests.TweetRetweets
{
    using Application.Dtos.TweetRetweets;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetRetweets;
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

    public class GetTweetRetweetsRepositoryTests
    { 
        
        [Fact]
        public void GetTweetRetweets_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweets = new FakeTweetRetweets { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweets);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                             var tweetRetweetsById = service.GetTweetRetweets(fakeTweetRetweets.TweetId);
                                tweetRetweetsById.TweetId.Should().Be(fakeTweetRetweets.TweetId);
                tweetRetweetsById.TweetRetweetId.Should().Be(fakeTweetRetweets.TweetRetweetId);
            }
        }
        
        [Fact]
        public async void GetTweetRetweetssAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto());

                             tweetRetweetsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsOne);
                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsTwo);
                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRetweetssAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            
                     fakeTweetRetweetsOne.TweetId = 1;
            fakeTweetRetweetsTwo.TweetId = 2;
            fakeTweetRetweetsThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { PageSize = 2 });

                             tweetRetweetsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsOne);
                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRetweetssAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            
                     fakeTweetRetweetsOne.TweetId = 1;
            fakeTweetRetweetsTwo.TweetId = 2;
            fakeTweetRetweetsThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { PageSize = 1, PageNumber = 2 });

                             tweetRetweetsRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                tweetRetweetsRepo.Should().ContainEquivalentOf(fakeTweetRetweetsTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRetweetssAsync_ListTweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetId = 2;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetId = 1;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { SortOrder = "TweetId" });

                             tweetRetweetsRepo.Should()
                    .ContainInOrder(fakeTweetRetweetsTwo, fakeTweetRetweetsOne, fakeTweetRetweetsThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRetweetssAsync_ListTweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetId = 2;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetId = 1;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { SortOrder = "-TweetId" });

                             tweetRetweetsRepo.Should()
                    .ContainInOrder(fakeTweetRetweetsThree, fakeTweetRetweetsOne, fakeTweetRetweetsTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRetweetssAsync_ListTweetRetweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetRetweetId = 2;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetRetweetId = 1;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetRetweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { SortOrder = "TweetRetweetId" });

                             tweetRetweetsRepo.Should()
                    .ContainInOrder(fakeTweetRetweetsTwo, fakeTweetRetweetsOne, fakeTweetRetweetsThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRetweetssAsync_ListTweetRetweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetRetweetId = 2;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetRetweetId = 1;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetRetweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { SortOrder = "-TweetRetweetId" });

                             tweetRetweetsRepo.Should()
                    .ContainInOrder(fakeTweetRetweetsThree, fakeTweetRetweetsOne, fakeTweetRetweetsTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTweetRetweetssAsync_FilterTweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetId = 1;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetId = 2;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { Filters = $"TweetId == {fakeTweetRetweetsTwo.TweetId}" });

                             tweetRetweetsRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRetweetssAsync_FilterTweetRetweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRetweetsDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsOne.TweetRetweetId = 1;

            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsTwo.TweetRetweetId = 2;

            var fakeTweetRetweetsThree = new FakeTweetRetweets { }.Generate();
            fakeTweetRetweetsThree.TweetRetweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo, fakeTweetRetweetsThree);
                context.SaveChanges();

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));

                var tweetRetweetsRepo = await service.GetTweetRetweetssAsync(new TweetRetweetsParametersDto { Filters = $"TweetRetweetId == {fakeTweetRetweetsTwo.TweetRetweetId}" });

                             tweetRetweetsRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}