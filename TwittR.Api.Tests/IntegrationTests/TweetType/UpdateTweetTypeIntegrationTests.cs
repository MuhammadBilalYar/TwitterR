
namespace TwittR.Api.Tests.IntegrationTests.TweetType
{
    using Application.Dtos.TweetType;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TweetType;
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
    public class UpdateTweetTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public UpdateTweetTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PatchTweetType204AndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TweetTypeProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            
            var expectedFinalObject = mapper.Map<TweetTypeDto>(fakeTweetTypeOne);
            expectedFinalObject.TweetTypeName = lookupVal;

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.TweetTypes.RemoveRange(context.TweetTypes);
                context.TweetTypes.AddRange(fakeTweetTypeOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var patchDoc = new JsonPatchDocument<TweetTypeForUpdateDto>();
            patchDoc.Replace(t => t.TweetTypeName, lookupVal);
            var serializedTweetTypeToUpdate = JsonConvert.SerializeObject(patchDoc);

                              var getResult = await client.GetAsync($"api/TweetTypes/?filters=TweetTypeName=={fakeTweetTypeOne.TweetTypeName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TweetTypeDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().TweetTypeId;

                     var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/TweetTypes/{id}")
            {
                Content = new StringContent(serializedTweetTypeToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TweetTypes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TweetTypeDto>>(checkResponseContent);

                     patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task PutTweetTypeReturnsBodyAndFieldsWereSuccessfullyUpdated()
        {
                     var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TweetTypeProfile>();
            }).CreateMapper();

            var fakeTweetTypeOne = new FakeTweetType { }.Generate();
            var expectedFinalObject = mapper.Map<TweetTypeDto>(fakeTweetTypeOne);
            expectedFinalObject.TweetTypeName = "Easily Identified Value For Test";

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TwittRDbContext> ();
                context.Database.EnsureCreated();

                context.TweetTypes.RemoveRange(context.TweetTypes);
                context.TweetTypes.AddRange(fakeTweetTypeOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var serializedTweetTypeToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

                              var getResult = await client.GetAsync($"api/TweetTypes/?filters=TweetTypeName=={fakeTweetTypeOne.TweetTypeName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TweetTypeDto>>>(getResponseContent);
            var id = getResponse?.Data.FirstOrDefault().TweetTypeId;

                     var putResult = await client.PutAsJsonAsync($"api/TweetTypes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TweetTypes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TweetTypeDto>>(checkResponseContent);

                     putResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
    } 
}