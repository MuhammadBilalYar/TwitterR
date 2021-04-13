
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
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateNotificationTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateNotificationTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostNotificationTypeReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeNotificationType = new FakeNotificationTypeDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/NotificationTypes", fakeNotificationType)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<NotificationTypeDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Data.NotificationTypeTitle.Should().Be(fakeNotificationType.NotificationTypeTitle);
            resultDto.Data.NotificationTypeDescription.Should().Be(fakeNotificationType.NotificationTypeDescription);
        }
    } 
}