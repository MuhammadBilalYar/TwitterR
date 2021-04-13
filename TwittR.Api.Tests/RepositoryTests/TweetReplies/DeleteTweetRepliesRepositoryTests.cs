
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

    public class DeleteTweetRepliesRepositoryTests
    { 
        
        [Fact]
        public void DeleteTweetReplies_ReturnsProperCount()
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

                var service = new TweetRepliesRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTweetReplies(fakeTweetRepliesTwo);

                context.SaveChanges();

                             var tweetRepliesList = context.TweetRepliess.ToList();

                tweetRepliesList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetRepliesList.Should().ContainEquivalentOf(fakeTweetRepliesOne);
                tweetRepliesList.Should().ContainEquivalentOf(fakeTweetRepliesThree);
                Assert.DoesNotContain(tweetRepliesList, t => t == fakeTweetRepliesTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}