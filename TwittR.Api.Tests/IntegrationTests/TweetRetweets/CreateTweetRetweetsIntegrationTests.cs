
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
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateTweetRetweetsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateTweetRetweetsIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact(Skip = "TODO")]
        public async Task PostTweetRetweetsReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTweetRetweets = new FakeTweetRetweetsDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/TweetRetweetss", fakeTweetRetweets)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<TweetRetweetsDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);

        }
    } 
}