using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleJobServer.Infrastructure.Resources
{
    public class CountryUpdateResource
    {
        public CountryUpdateResource()
        {
            Cities = new List<CityResource>();
        }

        public string EnglishName { get; set; }

        public string ChineseName { get; set; }

        public string Abbreviation { get; set; }

        public List<CityResource> Cities { get; set; }
    }

}
