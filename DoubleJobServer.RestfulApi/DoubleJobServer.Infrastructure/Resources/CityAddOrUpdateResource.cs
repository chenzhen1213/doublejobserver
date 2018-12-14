using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubleJobServer.Infrastructure.Resources
{
    //大部分情况下，PUT的验证可能和POST是一样的，但是有时还是不一样的，所以分别写两个ResourceModel对应POST和PUT的优势就体现出来了。
    //但是这两个类的大部分代码还是一样的，所以可以采取使用抽象父类的方法来去掉重复的代码
    public abstract class CityAddOrUpdateResource
    {
        [Display(Name = "名称")]
        [Required, StringLength(50, ErrorMessage = "{0}的程度不可超过{1}")]
        public virtual string Name { get; set; }

        [Display(Name = "描述")]
        [StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public virtual string Description { get; set; }
    }
}
