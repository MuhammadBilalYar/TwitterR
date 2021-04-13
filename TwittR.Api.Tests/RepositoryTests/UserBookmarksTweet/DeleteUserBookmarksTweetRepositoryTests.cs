
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

    public class DeleteUserBookmarksTweetRepositoryTests
    { 
        
        [Fact]
        public void DeleteUserBookmarksTweet_ReturnsProperCount()
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

                var service = new UserBookmarksTweetRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteUserBookmarksTweet(fakeUserBookmarksTweetTwo);

                context.SaveChanges();

                             var userBookmarksTweetList = context.UserBookmarksTweets.ToList();

                userBookmarksTweetList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                userBookmarksTweetList.Should().ContainEquivalentOf(fakeUserBookmarksTweetOne);
                userBookmarksTweetList.Should().ContainEquivalentOf(fakeUserBookmarksTweetThree);
                Assert.DoesNotContain(userBookmarksTweetList, u => u == fakeUserBookmarksTweetTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}