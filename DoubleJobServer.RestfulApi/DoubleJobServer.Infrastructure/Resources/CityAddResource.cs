using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoubleJobServer.Core.DomainModels;
using System.ComponentModel.DataAnnotations;


//如果一个HTTP请求造成了EFCore model的验证失败，如果返回500的话，感觉就不太正确。因为如果是500错误的话，就意味着是服务器出现了错误，而这实际上是API消费者（客户端）提交的数据有问题，是客户端的错误。所以返回的状态码应该是 4xx 系列。
//此外，目前这些验证规则是处于EFCore 的实体上的，而报告给API消费者的验证错误信息应该定义在Resource这一层面上，所以下面就为Resource model定义验证规则
//这种方式比较简单，但是把验证和Model混合到了一起，所以很多人还是不采用这种方式的

namespace DoubleJobServer.Infrastructure.Resources
{
    public class CityAddResource : CityAddOrUpdateResource
    {
   //     public int Id { get; set; }
//         public int CountryId { get; set; }
// 
//        [Display(Name = "名称")]
//        [Required, StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
//         public string Name { get; set; }
// 
//         [Display(Name = "描述")]
//         //[Required, StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
//         public string Description { get; set; }
// 
//         public Country Country { get; set; }
    }

}
