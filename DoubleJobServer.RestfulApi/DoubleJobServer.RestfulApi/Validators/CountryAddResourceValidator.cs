using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using DoubleJobServer.Infrastructure.Resources;

//为每一个类添加一个验证器Validator
namespace DoubleJobServer.RestfulApi.Validators
{
    public class CountryAddResourceValidator : AbstractValidator<CountryAddResource>
    {
        public CountryAddResourceValidator()
        {
            //大括号里面的字符串是参数（占位符），{PropertyName}就是属性的名字如果使用了WithName()方法，那就是WithName里面设定的别名；{MaxLength}就是指设定的最大长度约束的值
            RuleFor(c => c.EnglishName)
                .NotEmpty().WithName("英文名").WithMessage("{PropertyName}是必填项")
                .MaximumLength(100).WithMessage("{PropertyName}的长度不可超过{MaxLength}");

            RuleFor(c => c.ChineseName)
                .NotEmpty().WithName("中文名").WithMessage("{PropertyName}是必填项")
                .MaximumLength(100).WithMessage("{PropertyName}的长度不可超过{MaxLength}");

            RuleFor(c => c.Abbreviation)
                .NotEmpty().WithName("缩写").WithMessage("{PropertyName}是必填项")
                .MaximumLength(5).WithMessage("{PropertyName}的长度不可超过{MaxLength}");
        }
    }
}
