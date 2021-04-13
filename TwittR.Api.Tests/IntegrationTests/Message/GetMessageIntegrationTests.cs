
namespace TwittR.Api.Tests.IntegrationTests.Message
{
    using Application.Dtos.Message;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.Message;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class GetMessageIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetMessageIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetMessages_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeMessageOne = new FakeMessage { }.Generate();
            var fakeMessageTwo = new FakeMessage { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.Messages.AddRange(fakeMessageOne, fakeMessageTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/Messages")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<MessageDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeMessageOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeMessageTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetMessage_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeMessageOne = new FakeMessage { }.Generate();
            var fakeMessageTwo = new FakeMessage { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.Messages.AddRange(fakeMessageOne, fakeMessageTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/Messages/{fakeMessageOne.MessageId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<MessageDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeMessageOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}