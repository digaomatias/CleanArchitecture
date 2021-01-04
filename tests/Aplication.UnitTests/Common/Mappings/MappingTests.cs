using System;
using Application.Dto;
using Domain.Entities;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            TypeAdapterConfig typeAdapterConfig = new TypeAdapterConfig();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(typeAdapterConfig);
            services.AddScoped<IMapper, ServiceMapper>();

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            _mapper = scope.ServiceProvider.GetService<IMapper>();
        }


        [Theory]
        [InlineData(typeof(City), typeof(CityDto))]
        [InlineData(typeof(District), typeof(DistrictDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }

        [Fact]
        public void ShouldMappingCorrectly()
        {
            var city = new City { Id = 1, Name = "Bursa" };
            var cityDto = _mapper.Map<City, CityDto>(city);
            cityDto.Name.Should().Be("Bursa");
        }
    }
}
