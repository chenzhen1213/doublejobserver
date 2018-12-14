using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleJobServer.Core.DomainModels
{
    public class CountryResourceParameters : PaginationBase
    {
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
    }
}
