
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
    public class DeleteTweetTypeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeleteTweetTypeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task DeleteTweetType204AndFieldsWereSuccessfullyUpdated()
        {
                     var fakeTweetTypeOne = new FakeTweetType { }.Generate();

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

                              var getResult = await client.GetAsync($"api/TweetTypes/?filters=TweetTypeName=={fakeTweetTypeOne.TweetTypeName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TweetTypeDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().TweetTypeId;

                     var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/TweetTypes/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TweetTypes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TweetTypeDto>>(checkResponseContent);

                     deleteResult.StatusCode.Should().Be(204);
            checkResponse.Data.Should().Be(null);
        }
    } 
}