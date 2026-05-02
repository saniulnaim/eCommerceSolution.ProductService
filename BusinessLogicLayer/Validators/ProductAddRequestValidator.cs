using BusinessLogicLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Validators
{
    public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
    {
        public ProductAddRequestValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Category).IsInEnum().WithMessage("Invalid category value.");
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price must be a non-negative value.");
            RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("Quantity in stock must be a non-negative value.");
        }
    }
}
