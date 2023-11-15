using FluentValidation;
using POS.MediatR.Warehouse.Commands;

namespace POS.MediatR.Warehouse.Validators
{
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter name.");
        }
    }
}
