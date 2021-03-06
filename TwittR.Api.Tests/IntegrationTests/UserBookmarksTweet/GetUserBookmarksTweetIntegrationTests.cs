
namespace TwittR.Api.Tests.IntegrationTests.UserBookmarksTweet
{
    using Application.Dtos.UserBookmarksTweet;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.UserBookmarksTweet;
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
    public class GetUserBookmarksTweetIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetUserBookmarksTweetIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetUserBookmarksTweets_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/UserBookmarksTweets")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<UserBookmarksTweetDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeUserBookmarksTweetOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeUserBookmarksTweetTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetUserBookmarksTweet_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeUserBookmarksTweetOne = new FakeUserBookmarksTweet { }.Generate();
            var fakeUserBookmarksTweetTwo = new FakeUserBookmarksTweet { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.UserBookmarksTweets.AddRange(fakeUserBookmarksTweetOne, fakeUserBookmarksTweetTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/UserBookmarksTweets/{fakeUserBookmarksTweetOne.TwitterUserId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<UserBookmarksTweetDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeUserBookmarksTweetOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}