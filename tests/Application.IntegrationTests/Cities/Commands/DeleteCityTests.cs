using System.Threading.Tasks;
using Application.Cities.Commands.Create;
using Application.Cities.Commands.Delete;
using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.Cities.Commands
{
    [Collection("Integration tests")]
    public class DeleteCityTests : TestBase
    {
        [Fact]
        public void ShouldRequireValidCityId()
        {
            var command = new DeleteCityCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Fact]
        public async Task ShouldDeleteCity()
        {
            var city = await SendAsync(new CreateCityCommand
            {
                Name = "Kayseri"
            });

            await SendAsync(new DeleteCityCommand
            {
                Id = city.Data.Id
            });

            var list = await FindAsync<City>(city.Data.Id);

            list.Should().BeNull();
        }
    }
}
