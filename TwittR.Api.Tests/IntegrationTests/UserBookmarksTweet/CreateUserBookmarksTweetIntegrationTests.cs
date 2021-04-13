
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
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateUserBookmarksTweetIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateUserBookmarksTweetIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostUserBookmarksTweetReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeUserBookmarksTweet = new FakeUserBookmarksTweetDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/UserBookmarksTweets", fakeUserBookmarksTweet)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<UserBookmarksTweetDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);

        }
    } 
}