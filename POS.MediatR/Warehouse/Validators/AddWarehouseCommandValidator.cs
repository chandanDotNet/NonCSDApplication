using FluentValidation;
using POS.MediatR.Warehouse.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Warehouse.Validators
{
   public class AddWarehouseCommandValidator:  AbstractValidator<AddWarehouseCommand>
    {
        public AddWarehouseCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter name.");
        }
    }
}
