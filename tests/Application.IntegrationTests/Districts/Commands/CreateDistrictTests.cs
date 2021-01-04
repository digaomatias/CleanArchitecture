using System;
using System.Threading.Tasks;
using Application.Cities.Commands.Create;
using Application.Common.Exceptions;
using Application.Districts.Commands.Create;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.Districts.Commands
{
    [Collection("Integration tests")]
    public class CreateDistrictTests : TestBase
    {
        [Fact]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateDistrictCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Fact]
        public async Task ShouldCreateDistrict()
        {
            var city = await SendAsync(new CreateCityCommand
            {
                Name = "Bursa"
            });

            var userId = await RunAsDefaultUserAsync();

            var command = new CreateDistrictCommand
            {
                Name = "Karacabey",
                CityId = city.Data.Id
            };

            var result = await SendAsync(command);

            var list = await FindAsync<District>(result.Data.Id);

            list.Should().NotBeNull();
            list.Name.Should().Be(command.Name);
            list.Creator.Should().Be(userId);
            list.CreateDate.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
