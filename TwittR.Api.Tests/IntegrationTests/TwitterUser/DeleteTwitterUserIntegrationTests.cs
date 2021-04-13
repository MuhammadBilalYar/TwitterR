
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
    public class DeleteTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeleteTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task DeleteTwitterUser204AndFieldsWereSuccessfullyUpdated()
        {
                     var fakeTwitterUserOne = new FakeTwitterUser { }.Generate();

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

                              var getResult = await client.GetAsync($"api/TwitterUsers/?filters=FirstName=={fakeTwitterUserOne.FirstName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<TwitterUserDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().TwitterUserId;

                     var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/TwitterUsers/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

                     var checkResult = await client.GetAsync($"api/TwitterUsers/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<TwitterUserDto>>(checkResponseContent);

                     deleteResult.StatusCode.Should().Be(204);
            checkResponse.Data.Should().Be(null);
        }
    } 
}