using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace DoubleJobServer.Infrastructure.Resources
{
    //CityUpdateResource和CityAddResource所含有的属性是一样的，那么为什么不使用同一个类型呢？因为这两个对象的目的不同，责任不同，一个类只应该有一个责任（SRP）。但是你可以使用某个父类把相同的属性抽取出去，然后分别继承
    public class CityUpdateResource : CityAddOrUpdateResource
    {
        //         public string Name { get; set; }
        //         public int Id { get; set; }
        //         public int CountryId { get; set; }
        //         public Country Country { get; set; }

        [Required(ErrorMessage = "{0}是必填项")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
