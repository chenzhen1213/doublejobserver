using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleJobServer.Infrastructure.Resources
{
    public class CountryAddWithContinentResource
    {
        public CountryAddWithContinentResource()
        {
            Cities = new List<CityAddResource>();
        }

        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string Abbreviation { get; set; }

        public string Continent { get; set; }

        public List<CityAddResource> Cities { get; set; }
    }
}
