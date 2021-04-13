
namespace TwittR.Api.Tests.RepositoryTests.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TwitterUserFollowsTwitterUser;
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

    public class DeleteTwitterUserFollowsTwitterUserRepositoryTests
    { 
        
        [Fact]
        public void DeleteTwitterUserFollowsTwitterUser_ReturnsProperCount()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTwitterUserFollowsTwitterUser(fakeTwitterUserFollowsTwitterUserTwo);

                context.SaveChanges();

                             var twitterUserFollowsTwitterUserList = context.TwitterUserFollowsTwitterUsers.ToList();

                twitterUserFollowsTwitterUserList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                twitterUserFollowsTwitterUserList.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserOne);
                twitterUserFollowsTwitterUserList.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserThree);
                Assert.DoesNotContain(twitterUserFollowsTwitterUserList, t => t == fakeTwitterUserFollowsTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}