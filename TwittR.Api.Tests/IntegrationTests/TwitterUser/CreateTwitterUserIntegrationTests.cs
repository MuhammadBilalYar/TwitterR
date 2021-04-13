
namespace TwittR.Api.Tests.IntegrationTests.TwitterUser
{
    using Application.Dtos.TwitterUser;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TwitterUser;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostTwitterUserReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeTwitterUser = new FakeTwitterUserDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/TwitterUsers", fakeTwitterUser)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<TwitterUserDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Data.FirstName.Should().Be(fakeTwitterUser.FirstName);
            resultDto.Data.LastName.Should().Be(fakeTwitterUser.LastName);
            resultDto.Data.BirthDate.Should().Be(fakeTwitterUser.BirthDate);
            resultDto.Data.Email.Should().Be(fakeTwitterUser.Email);
            resultDto.Data.Phone.Should().Be(fakeTwitterUser.Phone);
            resultDto.Data.Login.Should().Be(fakeTwitterUser.Login);
            resultDto.Data.Password.Should().Be(fakeTwitterUser.Password);
            resultDto.Data.Photo_URL.Should().Be(fakeTwitterUser.Photo_URL);
            resultDto.Data.PublicUserId.Should().Be(fakeTwitterUser.PublicUserId);
        }
    } 
}