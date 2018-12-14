using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;

namespace DoubleJobServer.Core.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesForCountryAsync(int countryid);

        Task<City> GetCityForCountryAsync(int countryId, int cityId);

        void AddCityForCountry(int countryId, City city);

        void DeleteCity(City city);

        void UpdateCityForCountry(City city);
    }
}
