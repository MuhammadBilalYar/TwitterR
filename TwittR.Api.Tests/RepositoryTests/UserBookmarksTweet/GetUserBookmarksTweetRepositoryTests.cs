
namespace TwittR.Api.Tests.RepositoryTests.UserBookmarksTweet
{
    using Application.Dtos.UserBookmarksTweet;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.UserBookmarksTweet;
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

    public class GetUserBookmarksTweetRepositoryTests
    { 
        
        [Fact]
        public void GetUserBookmarksTweet_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweet = new FakeUserBookmarksTweet { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweet);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                             var userBookmarksTweetById = service.GetUserBookmarksTweet(fakeUserBookmarksTweet.TwitterUserId);
                                userBookmarksTweetById.TwitterUserId.Should().Be(fakeUserBookmarksTweet.TwitterUserId);
                userBookmarksTweetById.TweetId.Should().Be(fakeUserBookmarksTweet.TweetId);
            }
        }
        
        [Fact]
        public async void GetUserBookmarksTweetsAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto());

                             userBookmarksTweetRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetOne);
                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetTwo);
                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetUserBookmarksTweetsAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            
                     fakeUserBookmarksTweetOne.TwitterUserId = 1;
            fakeUserBookmarksTweetTwo.TwitterUserId = 2;
            fakeUserBookmarksTweetThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { PageSize = 2 });

                             userBookmarksTweetRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetOne);
                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetUserBookmarksTweetsAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            
                     fakeUserBookmarksTweetOne.TwitterUserId = 1;
            fakeUserBookmarksTweetTwo.TwitterUserId = 2;
            fakeUserBookmarksTweetThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { PageSize = 1, PageNumber = 2 });

                             userBookmarksTweetRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                userBookmarksTweetRepo.Should().ContainEquivalentOf(fakeUserBookmarksTweetTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetUserBookmarksTweetsAsync_ListTwitterUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TwitterUserId = 2;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TwitterUserId = 1;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { SortOrder = "TwitterUserId" });

                             userBookmarksTweetRepo.Should()
                    .ContainInOrder(fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetOne, fakeUserBookmarksTweetThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetUserBookmarksTweetsAsync_ListTwitterUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TwitterUserId = 2;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TwitterUserId = 1;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { SortOrder = "-TwitterUserId" });

                             userBookmarksTweetRepo.Should()
                    .ContainInOrder(fakeUserBookmarksTweetThree, fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetUserBookmarksTweetsAsync_ListTweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TweetId = 2;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TweetId = 1;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { SortOrder = "TweetId" });

                             userBookmarksTweetRepo.Should()
                    .ContainInOrder(fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetOne, fakeUserBookmarksTweetThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetUserBookmarksTweetsAsync_ListTweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TweetId = 2;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TweetId = 1;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { SortOrder = "-TweetId" });

                             userBookmarksTweetRepo.Should()
                    .ContainInOrder(fakeUserBookmarksTweetThree, fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetUserBookmarksTweetsAsync_FilterTwitterUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TwitterUserId = 1;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TwitterUserId = 2;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { Filters = $"TwitterUserId == {fakeUserBookmarksTweetTwo.TwitterUserId}" });

                             userBookmarksTweetRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetUserBookmarksTweetsAsync_FilterTweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserBookmarksTweetDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetOne.TweetId = 1;

            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetTwo.TweetId = 2;

            var fakeUserBookmarksTweetThree = new FakeUserBookmarksTweet { }.Generate();
            fakeUserBookmarksTweetThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo, fakeUserBookmarksTweetThree);
                context.SaveChanges();

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));

                var userBookmarksTweetRepo = await service.GetUserBookmarksTweetsAsync(new UserBookmarksTweetParametersDto { Filters = $"TweetId == {fakeUserBookmarksTweetTwo.TweetId}" });

                             userBookmarksTweetRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}