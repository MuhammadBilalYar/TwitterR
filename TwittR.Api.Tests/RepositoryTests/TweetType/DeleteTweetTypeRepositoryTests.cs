
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

    public class DeleteTweetTypeRepositoryTests
    { 
        
        [Fact]
        public void DeleteTweetType_ReturnsProperCount()
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

                var service = new TweetTypeRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTweetType(fakeTweetTypeTwo);

                context.SaveChanges();

                             var tweetTypeList = context.TweetTypes.ToList();

                tweetTypeList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                tweetTypeList.Should().ContainEquivalentOf(fakeTweetTypeOne);
                tweetTypeList.Should().ContainEquivalentOf(fakeTweetTypeThree);
                Assert.DoesNotContain(tweetTypeList, t => t == fakeTweetTypeTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}