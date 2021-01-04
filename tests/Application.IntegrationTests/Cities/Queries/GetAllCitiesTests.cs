using System.Threading.Tasks;
using Application.Cities.Queries.GetCities;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.Cities.Queries
{
    [Collection("Integration tests")]
    public class GetAllCitiesTests : TestBase
    {
        [Fact]
        public async Task ShouldReturnAllCities()
        {
            await AddAsync(new City
            {
                Name = "Manisa",
            });

            var query = new GetAllCitiesQuery();

            var result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Count.Should().BeGreaterThan(0);
        }
    }
}