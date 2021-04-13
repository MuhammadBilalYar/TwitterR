
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
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using Application.Mappings;
    using System.Text;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class UpdateTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public UpdateTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PatchTwitterUser204AndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TwitterUserProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            
            var expectedFinalObject = mapper.Map<TwitterUserDto>(fakeTwitterUserOne);
            expectedFinalObject.FirstName = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.TwitterUsers.RemoveRange(context.TwitterUsers);
                context.TwitterUsers.AddRange(fakeTwitterUserOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<TwitterUserForUpdateDto>();
            patchDoc.Replace(t => t.FirstName, lookupVal);
            var serializedTwitterUserToUpdate = JsonConvert.SerializeObject(patchDoc);

                              var getResult = await client.GetAsync($"api/TwitterUsers/?filters=FirstName=={fakeTwitterUserOne.FirstName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TwitterUserDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().TwitterUserId;

                     var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/TwitterUsers/{id}")
            {
                Content = new StringContent(serializedTwitterUserToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TwitterUsers/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TwitterUserDto>>(checkResponseContent);

                     patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task PutTwitterUserReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TwitterUserProfile>();
            }).CreateMapper();

            var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();
            var expectedFinalObject = mapper.Map<TwitterUserDto>(fakeTwitterUserOne);
            expectedFinalObject.FirstName = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.TwitterUsers.RemoveRange(context.TwitterUsers);
                context.TwitterUsers.AddRange(fakeTwitterUserOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedTwitterUserToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

                              var getResult = await client.GetAsync($"api/TwitterUsers/?filters=FirstName=={fakeTwitterUserOne.FirstName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TwitterUserDto>>>(getResponseContent);
            var id = getResponse?.Data.FirstOrDefault().TwitterUserId;

                     var putResult = await client.PutAsJsonAsync($"api/TwitterUsers/{id}", expectedFinalObject)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TwitterUsers/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TwitterUserDto>>(checkResponseContent);

                     putResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    } 
}