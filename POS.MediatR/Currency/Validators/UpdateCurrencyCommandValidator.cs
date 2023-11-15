using FluentValidation;
using POS.MediatR.Currency.Commands;

namespace POS.MediatR.Currency.Validators
{
    public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Currency Id is required.");
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter Name.");
            RuleFor(c => c.Symbol).NotEmpty().WithMessage("Please currency symbol.");
        }
    }
}