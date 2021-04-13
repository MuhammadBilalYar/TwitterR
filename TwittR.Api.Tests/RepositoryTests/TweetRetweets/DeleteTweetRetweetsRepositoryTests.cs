
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

    public class DeleteTweetRetweetsRepositoryTests
    { 
        
        [Fact]
        public void DeleteTweetRetweets_ReturnsProperCount()
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

                var service = new TweetRetweetsRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTweetRetweets(fakeTweetRetweetsTwo);

                context.SaveChanges();

                             var tweetRetweetsList = context.TweetRetweetss.ToList();

                tweetRetweetsList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetRetweetsList.Should().ContainEquivalentOf(fakeTweetRetweetsOne);
                tweetRetweetsList.Should().ContainEquivalentOf(fakeTweetRetweetsThree);
                Assert.DoesNotContain(tweetRetweetsList, t => t == fakeTweetRetweetsTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}