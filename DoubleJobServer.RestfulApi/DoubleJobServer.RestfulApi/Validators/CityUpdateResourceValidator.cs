using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using DoubleJobServer.Infrastructure.Resources;

namespace DoubleJobServer.RestfulApi.Validators
{
    public class CityUpdateResourceValidator
        : CityAddOrUpdateResourceValidator<CityUpdateResource>
    {
        public CityUpdateResourceValidator()
        {
            RuleFor(c => c.Description)
                .NotEmpty().WithName("描述").WithMessage("{PropertyName}是必填项");
        }
    }
}
