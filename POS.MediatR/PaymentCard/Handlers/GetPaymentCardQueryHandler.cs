using MediatR;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PaymentCard.Handlers
{
    public class GetPaymentCardQueryHandler : IRequestHandler<GetPaymentCardQuery, PaymentCardList>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        public GetPaymentCardQueryHandler(IPaymentCardRepository paymentCardRepository)
        {
            _paymentCardRepository = paymentCardRepository;
        }
        public async Task<PaymentCardList> Handle(GetPaymentCardQuery request, CancellationToken cancellationToken)
        {
            return await _paymentCardRepository.GetPaymentCards(request.PaymentCardResource);
        }
    }
}
