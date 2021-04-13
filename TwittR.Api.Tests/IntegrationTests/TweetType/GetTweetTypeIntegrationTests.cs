
namespace TwittR.Api.Tests.IntegrationTests.TweetType
{
    using Application.Dtos.TweetType;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetType;
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
    public class GetTweetTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTweetTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTweetTypes_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TweetTypes")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TweetTypeDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTweetTypeOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTweetTypeTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTweetType_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var fakeTweetTypeTwo = new FakeTweetType { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TweetTypes.AddRange(fakeTweetTypeOne, fakeTweetTypeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TweetTypes/{fakeTweetTypeOne.TweetTypeId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TweetTypeDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTweetTypeOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}