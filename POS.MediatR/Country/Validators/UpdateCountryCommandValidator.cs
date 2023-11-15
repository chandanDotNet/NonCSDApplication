using FluentValidation;
using POS.MediatR.Country.Commands;

namespace POS.MediatR.Country.Validators
{
   public class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
    {
        public UpdateCountryCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("Please enter countryId.");
            RuleFor(c => c.CountryName).NotEmpty().WithMessage("Please enter countryName.");
        }
    }
}