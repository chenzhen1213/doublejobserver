using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DoubleJobServer.Core.DomainModels;
using DoubleJobServer.Infrastructure.Resources;
using System.Linq;

namespace DoubleJobServer.Infrastructure.Configuration
{
    public class MappingProfile : Profile
    {
        //需要忽略Country的Cities属性的映射操作，然后把那部分代码写在AfterMap里面即可，这样在Action方法里面就简单了，可以使用Automapper了
        public MappingProfile()
        {
            CreateMap<Country, CountryResource>();
            CreateMap<CountryResource, Country>();
            CreateMap<CountryAddResource, Country>();
            CreateMap<CountryUpdateResource, Country>()
                .ForMember(c => c.Cities, opt => opt.Ignore())
                .AfterMap((countryUpdateResource, country) =>
                {
                    // Remove
                    var countryUpdateCityIds = countryUpdateResource.Cities.Select(x => x.Id).ToList();
                    var removedCities = country.Cities.Where(c => !countryUpdateCityIds.Contains(c.Id)).ToList();
                    foreach (var city in removedCities)
                    {
                        country.Cities.Remove(city);
                    }
                    // Add
                    var addedCityResources = countryUpdateResource.Cities.Where(x => x.Id == 0);
                    var addedCities = Mapper.Map<IEnumerable<City>>(addedCityResources);
                    foreach (var city in addedCities)
                    {
                        country.Cities.Add(city);
                    }
                    // Update or Unchanged
                    var maybeUpdateCities = country.Cities.Where(x => x.Id != 0).ToList();
                    foreach (var city in maybeUpdateCities)
                    {
                        var cityResource = countryUpdateResource.Cities.Single(x => x.Id == city.Id);
                        Mapper.Map(cityResource, city);
                    }
                });
            CreateMap<CountryAddWithContinentResource, Country>();

            CreateMap<City, CityResource>();
            CreateMap<CityResource, City>();
            CreateMap<CityAddResource, City>();
            CreateMap<CityUpdateResource, City>();
            CreateMap<City, CityUpdateResource>();
        }
    }
}
