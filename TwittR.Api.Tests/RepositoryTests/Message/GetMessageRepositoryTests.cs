
namespace TwittR.Api.Tests.RepositoryTests.Message
{
    using Application.Dtos.Message;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.Message;
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

    public class GetMessageRepositoryTests
    { 
        
        [Fact]
        public void GetMessage_ParametersMatchExpectedValues()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessage = new FakeMessage { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessage);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                             var messageById = service.GetMessage(fakeMessage.MessageId);
                                messageById.MessageId.Should().Be(fakeMessage.MessageId);
                messageById.SenderUserId.Should().Be(fakeMessage.SenderUserId);
                messageById.ReceiverUserId.Should().Be(fakeMessage.ReceiverUserId);
                messageById.MessageContent.Should().Be(fakeMessage.MessageContent);
                messageById.MessageSendDate.Should().Be(fakeMessage.MessageSendDate);
                messageById.MessageMediaURL.Should().Be(fakeMessage.MessageMediaURL);
                messageById.MessagePublicId.Should().Be(fakeMessage.MessagePublicId);
            }
        }
        
        [Fact]
        public async void GetMessagesAsync_CountMatchesAndContainsEquivalentObjects()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            var fakeMessageTwo = new FakeMessage { }.Generate();
            var fakeMessageThree = new FakeMessage { }.Generate();

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto());

                             messageRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                messageRepo.Should().ContainEquivalentOf(fakeMessageOne);
                messageRepo.Should().ContainEquivalentOf(fakeMessageTwo);
                messageRepo.Should().ContainEquivalentOf(fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetMessagesAsync_ReturnExpectedPageSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            var fakeMessageTwo = new FakeMessage { }.Generate();
            var fakeMessageThree = new FakeMessage { }.Generate();
            
                     fakeMessageOne.MessageId = 1;
            fakeMessageTwo.MessageId = 2;
            fakeMessageThree.MessageId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { PageSize = 2 });

                             messageRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                messageRepo.Should().ContainEquivalentOf(fakeMessageOne);
                messageRepo.Should().ContainEquivalentOf(fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetMessagesAsync_ReturnExpectedPageNumberAndSize()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            var fakeMessageTwo = new FakeMessage { }.Generate();
            var fakeMessageThree = new FakeMessage { }.Generate();
            
                     fakeMessageOne.MessageId = 1;
            fakeMessageTwo.MessageId = 2;
            fakeMessageThree.MessageId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { PageSize = 1, PageNumber = 2 });

                             messageRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                messageRepo.Should().ContainEquivalentOf(fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetMessagesAsync_ListMessageIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "MessageId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-MessageId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListSenderUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.SenderUserId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.SenderUserId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.SenderUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "SenderUserId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListSenderUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.SenderUserId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.SenderUserId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.SenderUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-SenderUserId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListReceiverUserIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.ReceiverUserId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.ReceiverUserId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.ReceiverUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "ReceiverUserId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListReceiverUserIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.ReceiverUserId = 2;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.ReceiverUserId = 1;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.ReceiverUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-ReceiverUserId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageContentSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageContent = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageContent = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageContent = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "MessageContent" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageContentSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageContent = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageContent = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageContent = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-MessageContent" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageSendDateSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageSendDate = DateTime.Now.AddDays(2);

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageSendDate = DateTime.Now.AddDays(1);

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageSendDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "MessageSendDate" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageSendDateSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageSendDate = DateTime.Now.AddDays(2);

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageSendDate = DateTime.Now.AddDays(1);

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageSendDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-MessageSendDate" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageMediaURLSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageMediaURL = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageMediaURL = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageMediaURL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "MessageMediaURL" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessageMediaURLSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageMediaURL = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageMediaURL = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageMediaURL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-MessageMediaURL" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessagePublicIdSortedInAscOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessagePublicId = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessagePublicId = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessagePublicId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "MessagePublicId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageTwo, fakeMessageOne, fakeMessageThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_ListMessagePublicIdSortedInDescOrder()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessagePublicId = "bravo";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessagePublicId = "alpha";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessagePublicId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { SortOrder = "-MessagePublicId" });

                             messageRepo.Should()
                    .ContainInOrder(fakeMessageThree, fakeMessageOne, fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetMessagesAsync_FilterMessageIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageId = 1;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageId = 2;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"MessageId == {fakeMessageTwo.MessageId}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterSenderUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.SenderUserId = 1;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.SenderUserId = 2;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.SenderUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"SenderUserId == {fakeMessageTwo.SenderUserId}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterReceiverUserIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.ReceiverUserId = 1;

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.ReceiverUserId = 2;

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.ReceiverUserId = 3;

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"ReceiverUserId == {fakeMessageTwo.ReceiverUserId}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterMessageContentListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageContent = "alpha";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageContent = "bravo";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageContent = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"MessageContent == {fakeMessageTwo.MessageContent}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterMessageSendDateListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageSendDate = DateTime.Now.AddDays(1);

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageSendDate = DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy"));

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageSendDate = DateTime.Now.AddDays(3);

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"MessageSendDate == {fakeMessageTwo.MessageSendDate}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterMessageMediaURLListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessageMediaURL = "alpha";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessageMediaURL = "bravo";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessageMediaURL = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"MessageMediaURL == {fakeMessageTwo.MessageMediaURL}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetMessagesAsync_FilterMessagePublicIdListWithExact()
        {
                     var dbOptions = new DbContextOptionsBuilder<TwittRDbContext>()
                .UseInMemoryDatabase(databaseName: $"MessageDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeMessageOne = new FakeMessage { }.Generate();
            fakeMessageOne.MessagePublicId = "alpha";

            var fakeMessageTwo = new FakeMessage { }.Generate();
            fakeMessageTwo.MessagePublicId = "bravo";

            var fakeMessageThree = new FakeMessage { }.Generate();
            fakeMessageThree.MessagePublicId = "charlie";

                     using (var context = new TwittRDbContext(dbOptions))
            {
                context.Messages.AddRange(fakeMessageOne, fakeMessageTwo, fakeMessageThree);
                context.SaveChanges();

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));

                var messageRepo = await service.GetMessagesAsync(new MessageParametersDto { Filters = $"MessagePublicId == {fakeMessageTwo.MessagePublicId}" });

                             messageRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}