
namespace TwittR.Api.Tests.IntegrationTests.TweetLikes
{
    using Application.Dtos.TweetLikes;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetLikes;
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
    public class GetTweetLikesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTweetLikesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTweetLikess_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TweetLikess")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TweetLikesDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTweetLikesOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTweetLikesTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTweetLikes_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetLikesOne = new FakeTweetLikes { }.Generate();
            var fakeTweetLikesTwo = new FakeTweetLikes { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetLikess.AddRange(fakeTweetLikesOne, fakeTweetLikesTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TweetLikess/{fakeTweetLikesOne.TweetId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TweetLikesDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTweetLikesOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}