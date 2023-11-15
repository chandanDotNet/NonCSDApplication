using FluentValidation;
using POS.MediatR.City.Commands;

namespace POS.MediatR.City.Validators
{
    public class AddCityCommandValidator: AbstractValidator<AddCityCommand>
    {
        public AddCityCommandValidator()
        {
            RuleFor(c => c.CityName).NotEmpty().WithMessage("Please enter cityName.");
            RuleFor(c => c.CountryId).NotEmpty().WithMessage("Please select country.");
        }
    }
}
