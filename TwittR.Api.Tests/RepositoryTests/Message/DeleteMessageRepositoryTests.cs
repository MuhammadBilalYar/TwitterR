
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

    public class DeleteMessageRepositoryTests
    { 
        
        [Fact]
        public void DeleteMessage_ReturnsProperCount()
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

                var service = new MessageRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteMessage(fakeMessageTwo);

                context.SaveChanges();

                             var messageList = context.Messages.ToList();

                messageList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                messageList.Should().ContainEquivalentOf(fakeMessageOne);
                messageList.Should().ContainEquivalentOf(fakeMessageThree);
                Assert.DoesNotContain(messageList, m => m == fakeMessageTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}