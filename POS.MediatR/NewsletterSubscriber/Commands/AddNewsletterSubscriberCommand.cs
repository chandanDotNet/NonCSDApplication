using POS.Data.Dto;
using MediatR;

namespace POS.MediatR.CommandAndQuery
{
    public class AddNewsletterSubscriberCommand : IRequest<NewsletterSubscriberDto>
    {
        public string Email { get; set; }
    }
}
