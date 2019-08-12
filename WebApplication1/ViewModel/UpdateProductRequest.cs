using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
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
