using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using DoubleJobServer.Infrastructure.Resources;

namespace DoubleJobServer.RestfulApi.Validators
{
    public class CityAddOrUpdateResourceValidator<T>
        : AbstractValidator<T> where T : CityAddOrUpdateResource
    {
        public CityAddOrUpdateResourceValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithName("名称")
                .WithMessage("{PropertyName}是必填项")
                .MaximumLength(50)
                .WithMessage("{PropertyName}的长度不能超过{MaxLength}")
                .NotEqual("中国").WithMessage("{PropertyName}的值不可以是{ComparisonValue}");

            RuleFor(c => c.Description)
                .MaximumLength(100).WithName("描述")
                .WithMessage("{PropertyName}的长度不能超过{MaxLength}");
        }
    }
}
