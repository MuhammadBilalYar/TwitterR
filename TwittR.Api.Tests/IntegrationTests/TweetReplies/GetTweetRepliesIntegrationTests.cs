
namespace TwittR.Api.Tests.IntegrationTests.TweetReplies
{
    using Application.Dtos.TweetReplies;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetReplies;
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
    public class GetTweetRepliesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTweetRepliesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTweetRepliess_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TweetRepliess")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TweetRepliesDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTweetRepliesOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTweetRepliesTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTweetReplies_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetRepliesOne = new FakeTweetReplies { }.Generate();
            var fakeTweetRepliesTwo = new FakeTweetReplies { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetRepliess.AddRange(fakeTweetRepliesOne, fakeTweetRepliesTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TweetRepliess/{fakeTweetRepliesOne.TweetId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TweetRepliesDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTweetRepliesOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}