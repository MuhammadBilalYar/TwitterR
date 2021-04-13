
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

    public class GetTwitterUserFollowsTwitterUserRepositoryTests
    { 
        
        [Fact]
        public void GetTwitterUserFollowsTwitterUser_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUser = new FakeTwitterUserFollowsTwitterUser { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUser);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                             var twitterUserFollowsTwitterUserById = service.GetTwitterUserFollowsTwitterUser(fakeTwitterUserFollowsTwitterUser.TwitterUserId);
                                twitterUserFollowsTwitterUserById.TwitterUserId.Should().Be(fakeTwitterUserFollowsTwitterUser.TwitterUserId);
                twitterUserFollowsTwitterUserById.FollowedTwitterUserId.Should().Be(fakeTwitterUserFollowsTwitterUser.FollowedTwitterUserId);
            }
        }
        
        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto());

                             twitterUserFollowsTwitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserOne);
                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserTwo);
                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            
                     fakeTwitterUserFollowsTwitterUserOne.TwitterUserId = 1;
            fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId = 2;
            fakeTwitterUserFollowsTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { PageSize = 2 });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserOne);
                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            
                     fakeTwitterUserFollowsTwitterUserOne.TwitterUserId = 1;
            fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId = 2;
            fakeTwitterUserFollowsTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { PageSize = 1, PageNumber = 2 });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                twitterUserFollowsTwitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ListTwitterUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.TwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { SortOrder = "TwitterUserId" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ListTwitterUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.TwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { SortOrder = "-TwitterUserId" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserFollowsTwitterUserThree, fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ListFollowedTwitterUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.FollowedTwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.FollowedTwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.FollowedTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { SortOrder = "FollowedTwitterUserId" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_ListFollowedTwitterUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.FollowedTwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.FollowedTwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.FollowedTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { SortOrder = "-FollowedTwitterUserId" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserFollowsTwitterUserThree, fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_FilterTwitterUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.TwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { Filters = $"TwitterUserId == {fakeTwitterUserFollowsTwitterUserTwo.TwitterUserId}" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUserFollowsTwitterUsersAsync_FilterFollowedTwitterUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserFollowsTwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserOne.FollowedTwitterUserId = 1;

            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserTwo.FollowedTwitterUserId = 2;

            var fakeTwitterUserFollowsTwitterUserThree = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            fakeTwitterUserFollowsTwitterUserThree.FollowedTwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo, fakeTwitterUserFollowsTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserFollowsTwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserFollowsTwitterUserRepo = await service.GetTwitterUserFollowsTwitterUsersAsync(new TwitterUserFollowsTwitterUserParametersDto { Filters = $"FollowedTwitterUserId == {fakeTwitterUserFollowsTwitterUserTwo.FollowedTwitterUserId}" });

                             twitterUserFollowsTwitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}