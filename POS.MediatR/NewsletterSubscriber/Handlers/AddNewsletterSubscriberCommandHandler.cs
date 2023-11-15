using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class AddNewsletterSubscriberCommandHandler : IRequestHandler<AddNewsletterSubscriberCommand, NewsletterSubscriberDto>
    {
        private readonly INewsletterSubscriberRepository _newsletterSubscriberRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;

        public AddNewsletterSubscriberCommandHandler(INewsletterSubscriberRepository newsletterSubscriberRepository,
            IUnitOfWork<POSDbContext> uow)
        {
            _newsletterSubscriberRepository = newsletterSubscriberRepository;
            _uow = uow;
        }
        public async Task<NewsletterSubscriberDto> Handle(AddNewsletterSubscriberCommand request, CancellationToken cancellationToken)
        {
            var newsletterSubscriber = new NewsletterSubscriber
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                CreatedDate = DateTime.UtcNow
            };

            _newsletterSubscriberRepository.Add(newsletterSubscriber);
            await _uow.SaveAsync();
            return new NewsletterSubscriberDto { };
        }
    }
}
