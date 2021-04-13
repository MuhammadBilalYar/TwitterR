
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
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class GetTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTwitterUsers_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TwitterUsers")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TwitterUserDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTwitterUserOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTwitterUserTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTwitterUser_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var fakeTwitterUserTwo = new FakeTwitterUser { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TwitterUsers.AddRange(fakeTwitterUserOne, fakeTwitterUserTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TwitterUsers/{fakeTwitterUserOne.TwitterUserId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TwitterUserDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTwitterUserOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}