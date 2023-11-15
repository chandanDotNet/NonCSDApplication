using FluentValidation;
using POS.MediatR.City.Commands;

namespace POS.MediatR.City.Validators
{
    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("CityId is required.");
            RuleFor(c => c.CityName).NotEmpty().WithMessage("Please enter cityName.");
            RuleFor(c => c.CountryId).NotEmpty().WithMessage("Please select country.");
        }
    }
}
