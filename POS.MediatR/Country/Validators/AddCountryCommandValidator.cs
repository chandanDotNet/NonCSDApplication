using FluentValidation;
using POS.MediatR.Country.Commands;

namespace POS.MediatR.Country.Validators
{
   public class AddCountryCommandValidator : AbstractValidator<AddCountryCommand>
    {
        public AddCountryCommandValidator()
        {
            RuleFor(c => c.CountryName).NotEmpty().WithMessage("Please enter countryName.");
        }
    }
}