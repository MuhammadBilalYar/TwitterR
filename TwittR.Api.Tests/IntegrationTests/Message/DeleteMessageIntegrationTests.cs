
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
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;    
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using Application.Mappings;
    using System.Text;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class DeleteMessageIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeleteMessageIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task DeleteMessage204AndFieldsWereSuccessfullyUpdated()
        {
                     var fakeMessageOne = new FakeMessage { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.Messages.RemoveRange(context.Messages);
                context.Messages.AddRange(fakeMessageOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

                              var getResult = await client.GetAsync($"api/Messages/?filters=MessageContent=={fakeMessageOne.MessageContent}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<MessageDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().MessageId;

                     var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/Messages/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/Messages/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<MessageDto>>(checkResponseContent);

                     deleteResult.StatusCode.Should().Be(204);
            checkResponse.Data.Should().Be(null);
        }
    } 
}