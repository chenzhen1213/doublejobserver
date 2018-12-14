using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;

namespace DoubleJobServer.Core.Interfaces
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<int> ids);

        //Task <IEnumerable<Country>> GetCountriesAsync(CountryResourceParameters parameters);
        Task<PaginatedList<Country>> GetCountriesAsync(CountryResourceParameters parameters);

        void AddCountry(Country country);

        Task<Country> GetCountryByIdAsync(int id,bool includeCities = false);

        Task<bool> CountryExistAsync(int id);

        void DeleteCountry(Country country);
        
    }
}
