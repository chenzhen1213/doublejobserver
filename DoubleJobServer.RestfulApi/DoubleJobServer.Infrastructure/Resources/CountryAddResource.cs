using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DoubleJobServer.Infrastructure.Resources
{
    public class CountryAddResource
    {
        public CountryAddResource()
        {
            Cities = new List<CityAddResource>();
        }

        public int Id { get; set; }

        [Display(Name = "英文名")]
        [Required, StringLength(100, ErrorMessage ="{0}的长度不可超过{1}")]
        public string EnglishName { get; set; }

        [Display(Name = "中文名")]
        [Required, StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string ChineseName { get; set; }

        [Display(Name = "缩写")]
        [Required, StringLength(100, ErrorMessage = "{0}的长度不可超过{1}")]
        public string Abbreviation { get; set; }

        public List<CityAddResource> Cities { get; set; }
    }
}
