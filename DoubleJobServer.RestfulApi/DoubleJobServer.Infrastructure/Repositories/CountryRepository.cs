using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using DoubleJobServer.Core.DomainModels;
using System.Threading.Tasks;
using DoubleJobServer.Core.Interfaces;
using System.Linq;

namespace DoubleJobServer.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly MyContext _myContext;

        public CountryRepository(MyContext myContext)
        {
            _myContext = myContext;
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _myContext.Countries.ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync(IEnumerable<int> ids)
        {
            return await _myContext.Countries.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        //需要确保的是要在迭代发生之前，使用Skip()和Take()以及Where()
        public async Task<PaginatedList<Country>> GetCountriesAsync(CountryResourceParameters parameters)
        {
            //             return await _myContext.Countries
            //                 .OrderBy(x => x.Id)
            //                 .Skip(parameters.PageSize * parameters.PageIndex)
            //                 .Take(parameters.PageSize)
            //                 .ToListAsync();

            var query = _myContext.Countries.OrderBy(x => x.Id);

            var count = await query.CountAsync();
            var items = await query
                .Skip(parameters.PageSize * parameters.PageIndex)
                .Take(parameters.PageSize).ToListAsync();
            return new PaginatedList<Country>(parameters.PageIndex, parameters.PageSize, count, items);
        }

        public void AddCountry(Country country)
        {
            _myContext.Countries.AddRange(country);
        }
        public async Task<Country> GetCountryByIdAsync(int id, bool includeCities = false)
        {
            if (!includeCities)
            {
                return await _myContext.Countries.FindAsync(id);
            }
            return await _myContext.Countries.Include(x => x.Cities).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CountryExistAsync(int id)
        {
            return await _myContext.Countries.FindAsync(id) != null;
        }

        public void DeleteCountry(Country country)
        {
            _myContext.Countries.Remove(country);
        }

    }
}
