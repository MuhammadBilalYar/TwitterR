
namespace TwittR.Api.Tests.IntegrationTests.NotificationType
{
    using Application.Dtos.NotificationType;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.NotificationType;
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
    public class GetNotificationTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetNotificationTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetNotificationTypes_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/NotificationTypes")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<NotificationTypeDto>>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeNotificationTypeOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeNotificationTypeTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetNotificationType_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var fakeNotificationTypeTwo = new FakeNotificationType { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext>();
                context.Database.EnsureCreated();

                             context.NotificationTypes.AddRange(fakeNotificationTypeOne, fakeNotificationTypeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/NotificationTypes/{fakeNotificationTypeOne.NotificationTypeId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<NotificationTypeDto>>(responseContent)?.Data;

                     result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeNotificationTypeOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}