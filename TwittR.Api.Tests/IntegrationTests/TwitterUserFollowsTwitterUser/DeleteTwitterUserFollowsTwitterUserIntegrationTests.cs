
namespace TwittR.Api.Tests.IntegrationTests.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using FluentAssertions;
    using TwittR.Api.Tests.Fakes.TwitterUserFollowsTwitterUser;
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
    public class DeleteTwitterUserFollowsTwitterUserIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeleteTwitterUserFollowsTwitterUserIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
    } 
}