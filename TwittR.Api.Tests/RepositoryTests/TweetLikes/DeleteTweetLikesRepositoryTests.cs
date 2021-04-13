
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

    public class DeleteTweetLikesRepositoryTests
    { 
        
        [Fact]
        public void DeleteTweetLikes_ReturnsProperCount()
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

                var service = new TweetLikesRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTweetLikes(fakeTweetLikesTwo);

                context.SaveChanges();

                             var tweetLikesList = context.TweetLikess.ToList();

                tweetLikesList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetLikesList.Should().ContainEquivalentOf(fakeTweetLikesOne);
                tweetLikesList.Should().ContainEquivalentOf(fakeTweetLikesThree);
                Assert.DoesNotContain(tweetLikesList, t => t == fakeTweetLikesTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}