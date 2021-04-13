
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
    public class UpdateMessageIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public UpdateMessageIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PatchMessage204AndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MessageProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeMessageOne = new FakeMessage { }.Generate();
            
            var expectedFinalObject = mapper.Map<MessageDto>(fakeMessageOne);
            expectedFinalObject.MessageContent = lookupVal;

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

            var patchDoc = new JsonPatchDocument<MessageForUpdateDto>();
            patchDoc.Replace(m => m.MessageContent, lookupVal);
            var serializedMessageToUpdate = JsonConvert.SerializeObject(patchDoc);

                              var getResult = await client.GetAsync($"api/Messages/?filters=MessageContent=={fakeMessageOne.MessageContent}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<MessageDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().MessageId;

                     var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/Messages/{id}")
            {
                Content = new StringContent(serializedMessageToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/Messages/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<MessageDto>>(checkResponseContent);

                     patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task PutMessageReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MessageProfile>();
            }).CreateMapper();

            var fakeMessageOne = new FakeMessage { }.Generate();
            var expectedFinalObject = mapper.Map<MessageDto>(fakeMessageOne);
            expectedFinalObject.MessageContent = "Easily Identified Value For Test";

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

            var serializedMessageToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

                              var getResult = await client.GetAsync($"api/Messages/?filters=MessageContent=={fakeMessageOne.MessageContent}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<MessageDto>>>(getResponseContent);
            var id = getResponse?.Data.FirstOrDefault().MessageId;

                     var putResult = await client.PutAsJsonAsync($"api/Messages/{id}", expectedFinalObject)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/Messages/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<MessageDto>>(checkResponseContent);

                     putResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    } 
}