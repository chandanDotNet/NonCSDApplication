using FluentValidation;
using POS.MediatR.Tax.Commands;

namespace POS.MediatR.Tax.Validators
{
    public class UpdateTaxCommandValidator : AbstractValidator<UpdateTaxCommand>
    {
        public UpdateTaxCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter Name.");
            RuleFor(c => c.Percentage).NotEmpty().WithMessage("Please enter percentage.");
        }
    }
}
