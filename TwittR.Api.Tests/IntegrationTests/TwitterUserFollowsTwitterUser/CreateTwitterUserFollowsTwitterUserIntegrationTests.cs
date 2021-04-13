
namespace TwittR.Api.Tests.IntegrationTests.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TwitterUserFollowsTwitterUser;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateTwitterUserFollowsTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateTwitterUserFollowsTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostTwitterUserFollowsTwitterUserReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTwitterUserFollowsTwitterUser = new FakeTwitterUserFollowsTwitterUserDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/TwitterUserFollowsTwitterUsers", fakeTwitterUserFollowsTwitterUser)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<TwitterUserFollowsTwitterUserDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);

        }
    } 
}