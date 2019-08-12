using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.ViewModel
{
    public class CreateProductRequest
    {     

        public string ProductName { get; set; }

        public int SupplierId { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Package { get; set; }

        public bool IsDiscontinued { get; set; }
    }

    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            //RuleFor(reg => reg.ProductName).EmailAddress().WithMessage("name is email format");
        }
    }
}