
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
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class GetTwitterUserFollowsTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetTwitterUserFollowsTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetTwitterUserFollowsTwitterUsers_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/TwitterUserFollowsTwitterUsers")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<TwitterUserFollowsTwitterUserDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeTwitterUserFollowsTwitterUserTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetTwitterUserFollowsTwitterUser_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeTwitterUserFollowsTwitterUserOne = new FakeTwitterUserFollowsTwitterUser { }.Generate();
            var fakeTwitterUserFollowsTwitterUserTwo = new FakeTwitterUserFollowsTwitterUser { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.TwitterUserFollowsTwitterUsers.AddRange(fakeTwitterUserFollowsTwitterUserOne, fakeTwitterUserFollowsTwitterUserTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/TwitterUserFollowsTwitterUsers/{fakeTwitterUserFollowsTwitterUserOne.TwitterUserId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<TwitterUserFollowsTwitterUserDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeTwitterUserFollowsTwitterUserOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}