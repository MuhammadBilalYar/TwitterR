
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
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateTweetLikesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateTweetLikesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact(Skip = "TODO")]
        public async Task PostTweetLikesReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTweetLikes = new FakeTweetLikesDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/TweetLikess", fakeTweetLikes)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<TweetLikesDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);

        }
    } 
}