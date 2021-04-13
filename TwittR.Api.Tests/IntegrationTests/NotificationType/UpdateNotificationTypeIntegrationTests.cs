
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
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using Application.Mappings;
    using System.Text;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class UpdateNotificationTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public UpdateNotificationTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PatchNotificationType204AndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<NotificationTypeProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            
            var expectedFinalObject = mapper.Map<NotificationTypeDto>(fakeNotificationTypeOne);
            expectedFinalObject.NotificationTypeTitle = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.NotificationTypes.RemoveRange(context.NotificationTypes);
                context.NotificationTypes.AddRange(fakeNotificationTypeOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<NotificationTypeForUpdateDto>();
            patchDoc.Replace(n => n.NotificationTypeTitle, lookupVal);
            var serializedNotificationTypeToUpdate = JsonConvert.SerializeObject(patchDoc);

                              var getResult = await client.GetAsync($"api/NotificationTypes/?filters=NotificationTypeTitle=={fakeNotificationTypeOne.NotificationTypeTitle}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<NotificationTypeDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().NotificationTypeId;

                     var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/NotificationTypes/{id}")
            {
                Content = new StringContent(serializedNotificationTypeToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/NotificationTypes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<NotificationTypeDto>>(checkResponseContent);

                     patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task PutNotificationTypeReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<NotificationTypeProfile>();
            }).CreateMapper();

            var fakeNotificationTypeOne = new FakeNotificationType { }.Generate();
            var expectedFinalObject = mapper.Map<NotificationTypeDto>(fakeNotificationTypeOne);
            expectedFinalObject.NotificationTypeTitle = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.NotificationTypes.RemoveRange(context.NotificationTypes);
                context.NotificationTypes.AddRange(fakeNotificationTypeOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedNotificationTypeToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

                              var getResult = await client.GetAsync($"api/NotificationTypes/?filters=NotificationTypeTitle=={fakeNotificationTypeOne.NotificationTypeTitle}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<NotificationTypeDto>>>(getResponseContent);
            var id = getResponse?.Data.FirstOrDefault().NotificationTypeId;

                     var putResult = await client.PutAsJsonAsync($"api/NotificationTypes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/NotificationTypes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<NotificationTypeDto>>(checkResponseContent);

                     putResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    } 
}