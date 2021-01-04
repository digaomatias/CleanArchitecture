using System.Threading.Tasks;
using Application.WeatherForecasts.Queries.GetCurrentWeatherForecastQuery;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.WeatherForecast.Queries
{
    [Collection("Integration tests")]
    public class GetCurrentWeatherTests : TestBase
    {
        [Fact]
        public async Task ShouldReturnCurrentWeather()
        {
            var query = new GetCurrentWeatherForecastQuery
            {
                Id = 2172797,
                Lat = 1,
                Lon = 1,
                Q = "Auckland"
            };

            var result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.weather.Count.Should().BeGreaterThan(0);
        }
    }
}