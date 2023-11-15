using FluentValidation;
using POS.MediatR.Currency.Commands;

namespace POS.MediatR.Currency.Validators
{
   public class AddCurrencyCommandValidator : AbstractValidator<AddCurrencyCommand>
    {
        public AddCurrencyCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter Name.");
            RuleFor(c => c.Symbol).NotEmpty().WithMessage("Please currency symbol.");
        }
    }
}