using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;

namespace DoubleJobServer.Infrastructure.Resources
{
    public class CityResource
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string Name { get; set; }

//        public Country Country { get; set; }
    }
}
