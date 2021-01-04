﻿using System;
using System.Threading.Tasks;
using Application.Cities.Commands.Create;
using Application.Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Xunit;
using static Application.IntegrationTests.IntegrationFixture;

namespace Application.IntegrationTests.Cities.Commands
{
    [Collection("Integration tests")]
    public class CreateCityTests : TestBase
    {
        [Fact]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateCityCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Fact]
        public async Task ShouldRequireUniqueName()
        {
            await SendAsync(new CreateCityCommand
            {
                Name = "Bursa"
            });

            var command = new CreateCityCommand
            {
                Name = "Bursa"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Fact]
        public async Task ShouldCreateCity()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateCityCommand
            {
                Name = "Kastamonu"
            };

            var result = await SendAsync(command);

            var list = await FindAsync<City>(result.Data.Id);

            list.Should().NotBeNull();
            list.Name.Should().Be(command.Name);
            list.Creator.Should().Be(userId);
            list.CreateDate.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}
