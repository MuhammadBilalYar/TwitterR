
namespace TwittR.Api.Tests.RepositoryTests.TweetReplies
{
    using Application.Dtos.TweetReplies;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetReplies;
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

    public class GetTweetRepliesRepositoryTests
    { 
        
        [Fact]
        public void GetTweetReplies_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetReplies = new FakeTweetReplies { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetReplies);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                             var tweetRepliesById = service.GetTweetReplies(fakeTweetReplies.TweetId);
                                tweetRepliesById.TweetId.Should().Be(fakeTweetReplies.TweetId);
                tweetRepliesById.ReplyTweetId.Should().Be(fakeTweetReplies.ReplyTweetId);
            }
        }
        
        [Fact]
        public async void GetTweetRepliessAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto());

                             tweetRepliesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesOne);
                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesTwo);
                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRepliessAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            
                     fakeTweetRepliesOne.TweetId = 1;
            fakeTweetRepliesTwo.TweetId = 2;
            fakeTweetRepliesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { PageSize = 2 });

                             tweetRepliesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesOne);
                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRepliessAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            
                     fakeTweetRepliesOne.TweetId = 1;
            fakeTweetRepliesTwo.TweetId = 2;
            fakeTweetRepliesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { PageSize = 1, PageNumber = 2 });

                             tweetRepliesRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                tweetRepliesRepo.Should().ContainEquivalentOf(fakeTweetRepliesTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTweetRepliessAsync_ListTweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.TweetId = 2;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.TweetId = 1;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { SortOrder = "TweetId" });

                             tweetRepliesRepo.Should()
                    .ContainInOrder(fakeTweetRepliesTwo, fakeTweetRepliesOne, fakeTweetRepliesThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRepliessAsync_ListTweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.TweetId = 2;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.TweetId = 1;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { SortOrder = "-TweetId" });

                             tweetRepliesRepo.Should()
                    .ContainInOrder(fakeTweetRepliesThree, fakeTweetRepliesOne, fakeTweetRepliesTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRepliessAsync_ListReplyTweetIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.ReplyTweetId = 2;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.ReplyTweetId = 1;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.ReplyTweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { SortOrder = "ReplyTweetId" });

                             tweetRepliesRepo.Should()
                    .ContainInOrder(fakeTweetRepliesTwo, fakeTweetRepliesOne, fakeTweetRepliesThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRepliessAsync_ListReplyTweetIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.ReplyTweetId = 2;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.ReplyTweetId = 1;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.ReplyTweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { SortOrder = "-ReplyTweetId" });

                             tweetRepliesRepo.Should()
                    .ContainInOrder(fakeTweetRepliesThree, fakeTweetRepliesOne, fakeTweetRepliesTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTweetRepliessAsync_FilterTweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.TweetId = 1;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.TweetId = 2;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.TweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { Filters = $"TweetId == {fakeTweetRepliesTwo.TweetId}" });

                             tweetRepliesRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTweetRepliessAsync_FilterReplyTweetIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TweetRepliesDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesOne.ReplyTweetId = 1;

            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesTwo.ReplyTweetId = 2;

            var fakeTweetRepliesThree = new FakeTweetReplies { }.Generate();
            fakeTweetRepliesThree.ReplyTweetId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo, fakeTweetRepliesThree);
                context.SaveChanges();

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));

                var tweetRepliesRepo = await service.GetTweetRepliessAsync(new TweetRepliesParametersDto { Filters = $"ReplyTweetId == {fakeTweetRepliesTwo.ReplyTweetId}" });

                             tweetRepliesRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}