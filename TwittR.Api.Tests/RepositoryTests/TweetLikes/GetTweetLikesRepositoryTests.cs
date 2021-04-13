
namespace TwittR.Api.Tests.RepositoryTests.TweetLikes
{
    using Application.Dtos.TweetLikes;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetLikes;
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

    public class GetTweetLikesRepositoryTests
    { 
        
        [Fact]
        public void GetTweetLikes_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikes = new FakeTweetLikes { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikes);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                             var tweetLikesById = service.GetTweetLikes(fakeTweetLikes.TweetId);
                                tweetLikesById.TweetId.Should().Be(fakeTweetLikes.TweetId);
                tweetLikesById.LikerTwitterUserId.Should().Be(fakeTweetLikes.LikerTwitterUserId);
            }
        }
        
        [Fact]
        public async void GetTweetLikessAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto());

                             tweetLikesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesOne);
                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesTwo);
                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetLikessAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            
                     fakeTweetLikesOne.TweetId = 1;
            fakeTweetLikesTwo.TweetId = 2;
            fakeTweetLikesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { PageSize = 2 });

                             tweetLikesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesOne);
                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetLikessAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            
                     fakeTweetLikesOne.TweetId = 1;
            fakeTweetLikesTwo.TweetId = 2;
            fakeTweetLikesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { PageSize = 1, PageNumber = 2 });

                             tweetLikesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                tweetLikesRepo.Should().ContainEquivalentOf(fakeTweetLikesTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetLikessAsync_ListTweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.TweetId = 2;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.TweetId = 1;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { SortOrder = "TweetId" });

                             tweetLikesRepo.Should()
                    .ContainInOrder(fakeTweetLikesTwo, fakeTweetLikesOne, fakeTweetLikesThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetLikessAsync_ListTweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.TweetId = 2;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.TweetId = 1;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { SortOrder = "-TweetId" });

                             tweetLikesRepo.Should()
                    .ContainInOrder(fakeTweetLikesThree, fakeTweetLikesOne, fakeTweetLikesTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetLikessAsync_ListLikerTwitterUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.LikerTwitterUserId = 2;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.LikerTwitterUserId = 1;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.LikerTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { SortOrder = "LikerTwitterUserId" });

                             tweetLikesRepo.Should()
                    .ContainInOrder(fakeTweetLikesTwo, fakeTweetLikesOne, fakeTweetLikesThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetLikessAsync_ListLikerTwitterUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.LikerTwitterUserId = 2;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.LikerTwitterUserId = 1;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.LikerTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { SortOrder = "-LikerTwitterUserId" });

                             tweetLikesRepo.Should()
                    .ContainInOrder(fakeTweetLikesThree, fakeTweetLikesOne, fakeTweetLikesTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTweetLikessAsync_FilterTweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.TweetId = 1;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.TweetId = 2;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { Filters = $"TweetId == {fakeTweetLikesTwo.TweetId}" });

                             tweetLikesRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetLikessAsync_FilterLikerTwitterUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetLikesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            fakeTweetLikesOne.LikerTwitterUserId = 1;

            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();
            fakeTweetLikesTwo.LikerTwitterUserId = 2;

            var fakeTweetLikesThree = new FakeTweetLikes { }.Generate();
            fakeTweetLikesThree.LikerTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo, fakeTweetLikesThree);
                context.SaveChanges();

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));

                var tweetLikesRepo = await service.GetTweetLikessAsync(new TweetLikesParametersDto { Filters = $"LikerTwitterUserId == {fakeTweetLikesTwo.LikerTwitterUserId}" });

                             tweetLikesRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}