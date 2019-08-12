using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.ViewModel
{
    [Validator(typeof(UpdateProductRequestValidator))]
    public class UpdateProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateProductRequestValidator: AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(reg => reg.Id).NotEqual(Guid.Empty).WithMessage("Id can not empty");
            RuleFor(reg => reg.Name).EmailAddress().WithMessage("name is email format");
        }
    }
}
