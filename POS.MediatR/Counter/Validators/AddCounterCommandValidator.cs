using FluentValidation;
using POS.MediatR.Counter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Validators
{
    public class AddCounterCommandValidator : AbstractValidator<AddCounterCommand>
    {
        public AddCounterCommandValidator()
        {
            RuleFor(c => c.CounterName).NotEmpty().WithMessage("Please enter counterName.");
        }
    }
}
