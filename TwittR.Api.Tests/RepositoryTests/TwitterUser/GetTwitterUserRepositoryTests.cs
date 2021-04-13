
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

    public class GetTwitterUserRepositoryTests
    { 
        
        [Fact]
        public void GetTwitterUser_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUser = new FakeTwitterUser { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUser);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                             var twitterUserById = service.GetTwitterUser(fakeTwitterUser.TwitterUserId);
                                twitterUserById.TwitterUserId.Should().Be(fakeTwitterUser.TwitterUserId);
                twitterUserById.FirstName.Should().Be(fakeTwitterUser.FirstName);
                twitterUserById.LastName.Should().Be(fakeTwitterUser.LastName);
                twitterUserById.BirthDate.Should().Be(fakeTwitterUser.BirthDate);
                twitterUserById.Email.Should().Be(fakeTwitterUser.Email);
                twitterUserById.Phone.Should().Be(fakeTwitterUser.Phone);
                twitterUserById.Login.Should().Be(fakeTwitterUser.Login);
                twitterUserById.Password.Should().Be(fakeTwitterUser.Password);
                twitterUserById.Photo_URL.Should().Be(fakeTwitterUser.Photo_URL);
                twitterUserById.PublicUserId.Should().Be(fakeTwitterUser.PublicUserId);
            }
        }
        
        [Fact]
        public async void GetTwitterUsersAsync_CountMatchesAndContainsEquivalentObjects()
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
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto());

                             twitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserOne);
                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserTwo);
                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUsersAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            
                     fakeTwitterUserOne.TwitterUserId = 1;
            fakeTwitterUserTwo.TwitterUserId = 2;
            fakeTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { PageSize = 2 });

                             twitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserOne);
                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUsersAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            
                     fakeTwitterUserOne.TwitterUserId = 1;
            fakeTwitterUserTwo.TwitterUserId = 2;
            fakeTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { PageSize = 1, PageNumber = 2 });

                             twitterUserRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                twitterUserRepo.Should().ContainEquivalentOf(fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetTwitterUsersAsync_ListTwitterUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.TwitterUserId = 2;

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.TwitterUserId = 1;

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "TwitterUserId" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListTwitterUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.TwitterUserId = 2;

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.TwitterUserId = 1;

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-TwitterUserId" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListFirstNameSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.FirstName = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.FirstName = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.FirstName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "FirstName" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListFirstNameSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.FirstName = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.FirstName = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.FirstName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-FirstName" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListLastNameSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.LastName = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.LastName = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.LastName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "LastName" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListLastNameSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.LastName = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.LastName = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.LastName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-LastName" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListBirthDateSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.BirthDate = DateTime.Now.AddDays(2);

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.BirthDate = DateTime.Now.AddDays(1);

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.BirthDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "BirthDate" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListBirthDateSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.BirthDate = DateTime.Now.AddDays(2);

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.BirthDate = DateTime.Now.AddDays(1);

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.BirthDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-BirthDate" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListEmailSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Email = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Email = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Email = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "Email" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListEmailSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Email = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Email = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Email = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-Email" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPhoneSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Phone = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Phone = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Phone = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "Phone" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPhoneSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Phone = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Phone = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Phone = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-Phone" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListLoginSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Login = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Login = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Login = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "Login" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListLoginSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Login = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Login = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Login = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-Login" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPasswordSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Password = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Password = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Password = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "Password" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPasswordSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Password = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Password = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Password = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-Password" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPhoto_URLSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Photo_URL = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Photo_URL = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Photo_URL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "Photo_URL" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPhoto_URLSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Photo_URL = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Photo_URL = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Photo_URL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-Photo_URL" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPublicUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.PublicUserId = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.PublicUserId = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.PublicUserId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "PublicUserId" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserTwo, fakeTwitterUserOne, fakeTwitterUserThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_ListPublicUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.PublicUserId = "bravo";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.PublicUserId = "alpha";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.PublicUserId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { SortOrder = "-PublicUserId" });

                             twitterUserRepo.Should()
                    .ContainInOrder(fakeTwitterUserThree, fakeTwitterUserOne, fakeTwitterUserTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetTwitterUsersAsync_FilterTwitterUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.TwitterUserId = 1;

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.TwitterUserId = 2;

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.TwitterUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"TwitterUserId == {fakeTwitterUserTwo.TwitterUserId}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterFirstNameListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.FirstName = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.FirstName = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.FirstName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"FirstName == {fakeTwitterUserTwo.FirstName}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterLastNameListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.LastName = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.LastName = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.LastName = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"LastName == {fakeTwitterUserTwo.LastName}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterBirthDateListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.BirthDate = DateTime.Now.AddDays(1);

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.BirthDate = DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy"));

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.BirthDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"BirthDate == {fakeTwitterUserTwo.BirthDate}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterEmailListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Email = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Email = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Email = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"Email == {fakeTwitterUserTwo.Email}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterPhoneListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Phone = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Phone = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Phone = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"Phone == {fakeTwitterUserTwo.Phone}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterLoginListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Login = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Login = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Login = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"Login == {fakeTwitterUserTwo.Login}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterPasswordListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Password = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Password = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Password = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"Password == {fakeTwitterUserTwo.Password}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterPhoto_URLListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.Photo_URL = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.Photo_URL = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.Photo_URL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"Photo_URL == {fakeTwitterUserTwo.Photo_URL}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetTwitterUsersAsync_FilterPublicUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"TwitterUserDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            fakeTwitterUserOne.PublicUserId = "alpha";

            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();
            fakeTwitterUserTwo.PublicUserId = "bravo";

            var fakeTwitterUserThree = new FakeTwitterUser { }.Generate();
            fakeTwitterUserThree.PublicUserId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo, fakeTwitterUserThree);
                context.SaveChanges();

                var service = new TwitterUserRepository(context, new SieveProcessor(sieveOptions));

                var twitterUserRepo = await service.GetTwitterUsersAsync(new TwitterUserParametersDto { Filters = $"PublicUserId == {fakeTwitterUserTwo.PublicUserId}" });

                             twitterUserRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}