
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
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateTweetRepliesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateTweetRepliesIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact(Skip = "TODO")]
        public async Task PostTweetRepliesReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTweetReplies = new FakeTweetRepliesDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/TweetRepliess", fakeTweetReplies)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<TweetRepliesDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);

        }
    } 
}