
namespace TwittR.Api.Tests.IntegrationTests.TweetRetweets
{
    using Application.Dtos.TweetRetweets;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetRetweets;
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
    public class GetTweetRetweetsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTweetRetweetsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTweetRetweetss_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TweetRetweetss")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TweetRetweetsDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTweetRetweetsOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTweetRetweetsTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTweetRetweets_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetRetweetsOne = new FakeTweetRetweets { }.Generate();
            var fakeTweetRetweetsTwo = new FakeTweetRetweets { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetRetweetss.AddRange(fakeTweetRetweetsOne, fakeTweetRetweetsTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TweetRetweetss/{fakeTweetRetweetsOne.TweetId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TweetRetweetsDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTweetRetweetsOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}