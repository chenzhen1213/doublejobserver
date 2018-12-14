using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//用REST的术语来说, 我们把客户端请求服务器返回的对象叫做资源(Resources). 所以我会在MyRestful.Api项目里建立一个Resources文件夹, 并创建一个类叫做CountryResource.cs (以前我把它叫ViewModel或Dto, 在这里我叫它Resource, 都是一个意思):
namespace DoubleJobServer.Infrastructure.Resources
{
    public class CountryResource
    {
        public CountryResource()
        {
            Cities = new List<CityResource>();
        }

        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ChineseName { get; set; }
        public string Abbreviation { get; set; }

        public List<CityResource> Cities { get; set; }
    }
}