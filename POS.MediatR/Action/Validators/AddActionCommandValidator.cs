using POS.MediatR.CommandAndQuery;
using FluentValidation;

namespace POS.MediatR.Validators
{
    public class AddActionCommandValidator: AbstractValidator<AddActionCommand>
    {
        public AddActionCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
