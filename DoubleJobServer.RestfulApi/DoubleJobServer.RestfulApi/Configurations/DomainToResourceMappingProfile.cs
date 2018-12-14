using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using DoubleJobServer.Infrastructure.Resources;
using DoubleJobServer.Core.DomainModels;

    //把MyContext查询出来的Country映射成CountryResource, 你可以手动编写映射关系

namespace DoubleJobServer.RestfulApi.Configurations
{
    public class DomainToResourceMappingProfile : Profile
    {
        public override string ProfileName { get; } = "DomainToResourceMappings";

        public DomainToResourceMappingProfile()
        {
            CreateMap<Country, CountryResource>();
        }
    }
}
