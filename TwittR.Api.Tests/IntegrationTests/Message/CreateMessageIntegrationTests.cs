
namespace TwittR.Api.Tests.IntegrationTests.Message
{
    using Application.Dtos.Message;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.Message;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreateMessageIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateMessageIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostMessageReturnsSuccessCodeAndResourceWithAccurateFields()
        {
                     var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeMessage = new FakeMessageDto().Generate();

                     var httpResponse = await client.PostAsJsonAsync("api/Messages", fakeMessage)
                .ConfigureAwait(false);

                     httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<MessageDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Data.SenderUserId.Should().Be(fakeMessage.SenderUserId);
            resultDto.Data.ReceiverUserId.Should().Be(fakeMessage.ReceiverUserId);
            resultDto.Data.MessageContent.Should().Be(fakeMessage.MessageContent);
            resultDto.Data.MessageSendDate.Should().Be(fakeMessage.MessageSendDate);
            resultDto.Data.MessageMediaURL.Should().Be(fakeMessage.MessageMediaURL);
            resultDto.Data.MessagePublicId.Should().Be(fakeMessage.MessagePublicId);
        }
    } 
}