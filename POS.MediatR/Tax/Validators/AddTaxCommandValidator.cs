using FluentValidation;
using POS.MediatR.Tax.Commands;

namespace POS.MediatR.Tax.Validators
{
    public class AddTaxCommandValidator : AbstractValidator<AddTaxCommand>
    {
        public AddTaxCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter Name.");
            RuleFor(c => c.Percentage).NotEmpty().WithMessage("Please enter percentage.");
        }
    }
}