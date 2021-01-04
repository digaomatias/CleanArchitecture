using System;
using System.Threading.Tasks;
using Application.Cities.Commands.Create;
using Application.Cities.Commands.Update;
using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.Cities.Commands
{
    [Collection("Integration tests")]
    public class UpdateCityTests : TestBase
    {
        [Fact]
        public void ShouldRequireValidCityId()
        {
            var command = new UpdateCityCommand
            {
                Id = 99,
                Name = "Kayseri"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task ShouldRequireUniqueName()
        {
            var city = await SendAsync(new CreateCityCommand
            {
                Name = "Malatya"
            });

            await SendAsync(new CreateCityCommand
            {
                Name = "Denizli"
            });

            var command = new UpdateCityCommand
            {
                Id = city.Data.Id,
                Name = "Denizli"
            };

            FluentActions.Invoking(() =>
                    SendAsync(command))
                .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name"))
                .And.Errors["Name"].Should().Contain("The specified city already exists. If you just want to activate the city leave the name field blank!");
        }

        [Fact]
        public async Task ShouldUpdateCity()
        {
            var userId = await RunAsDefaultUserAsync();

            var result = await SendAsync(new CreateCityCommand
            {
                Name = "Kayyysseri"
            });

            var command = new UpdateCityCommand
            {
                Id = result.Data.Id,
                Name = "Kayseri"
            };

            await SendAsync(command);

            var city = await FindAsync<City>(result.Data.Id);

            city.Name.Should().Be(command.Name);
            city.Modifier.Should().NotBeNull();
            city.Modifier.Should().Be(userId);
            city.ModifyDate.Should().NotBeNull();
            city.ModifyDate.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
