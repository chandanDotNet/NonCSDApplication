using POS.MediatR.CommandAndQuery;
using FluentValidation;

namespace POS.MediatR.Validators
{
    public class UpdateInquiryNoteCommandValidator : AbstractValidator<UpdateInquiryNoteCommand>
    {
        public UpdateInquiryNoteCommandValidator()
        {
            RuleFor(c => c.Note).NotEmpty().WithMessage("Note is required");
            RuleFor(c => c.InquiryId).NotEmpty().WithMessage("Inquiry Id is required");
        }
    }
}
