using System;
using System.Collections.Generic;
using System.Text;

using DoubleJobServer.Core.Interfaces;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;
using Microsoft.EntityFrameworkCore;

using DoubleJobServer.Core.Interfaces;

namespace DoubleJobServer.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly MyContext _myContext;

        public CityRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<IEnumerable<City>> GetCitiesForCountryAsync(int countryid)
        {
            return await _myContext.Cities.ToListAsync();
        }

        public async Task<City> GetCityForCountryAsync(int countryId, int cityId)
        {
            return await _myContext.Cities.FindAsync(cityId);
        }

        public void AddCityForCountry(int countryId, City city)
        {
            city.CountryId = countryId;
            _myContext.Cities.AddRange(city);
        }

        public void DeleteCity(City city)
        {
            _myContext.Cities.Remove(city);
        }

        public void UpdateCityForCountry(City city)
        {
            //这个是DbContext的方法而不是DbSet的方法，它会追踪city，然后把它的ModelState设置为Modified。
            _myContext.Update(city);
        }
    }
}
