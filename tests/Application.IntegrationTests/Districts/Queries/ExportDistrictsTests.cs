using System;
using System.Threading.Tasks;
using Application.Cities.Commands.Create;
using Application.Common.Exceptions;
using Application.Common.Security;
using Application.Districts.Commands.Create;
using Application.Districts.Queries;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Districts.Queries
{
    using static IntegrationFixture;

    [Collection("Integration tests")]
    public class ExportDistrictsTests : TestBase
    {
        [Fact]
        public void ShouldDenyAnonymousUser()
        {
            var query = new ExportDistrictsQuery();

            query.GetType().Should().BeDecoratedWith<AuthorizeAttribute>();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task ShouldDenyNonAdministrator()
        {
            await RunAsDefaultUserAsync();

            var query = new ExportDistrictsQuery();

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<ForbiddenAccessException>();
        }

        [Fact]
        public async Task ShouldAllowAdministrator()
        {
            await RunAsAdministratorAsync();

            var city = await SendAsync(new CreateCityCommand
            {
                Name = "Çanakkale"
            });

            var result = await SendAsync(new CreateDistrictCommand
            {
                Name = "Çan",
                CityId = city.Data.Id
            });

            var query = new ExportDistrictsQuery
            {
                CityId = result.Data.Id
            };

            FluentActions.Invoking(() => SendAsync(query))
                .Should().NotThrow<ForbiddenAccessException>();
        }
    }
}
