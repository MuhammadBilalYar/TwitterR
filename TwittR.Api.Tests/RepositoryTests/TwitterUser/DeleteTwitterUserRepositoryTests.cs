
namespace TwittR.Api.Tests.RepositoryTests.TwitterUser
{
    using Application.Dtos.TwitterUser;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TwitterUser;
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

    public class DeleteTwitterUserRepositoryTests
    { 
        
        [Fact]
        public void DeleteTwitterUser_ReturnsProperCount()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteTwitterUser(fakeTwitterUserTwo);

                context.SaveChanges();

                             var twitterUserList = context.TwitterUsers.ToList();

                twitterUserList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                twitterUserList.Should().ContainEquivalentOf(fakeTwitterUserOne);
                twitterUserList.Should().ContainEquivalentOf(fakeTwitterUserThree);
                Assert.DoesNotContain(twitterUserList, t => t == fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}