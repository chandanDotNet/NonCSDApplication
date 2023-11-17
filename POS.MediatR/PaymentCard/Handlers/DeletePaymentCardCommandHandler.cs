using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.PaymentCard.Commands;
using POS.MediatR.PaymentCard.Handlers;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.PaymentCard.Handlers
{
    public class DeletePaymentCardCommandHandler : IRequestHandler<DeletePaymentCardCommand, ServiceResponse<bool>>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeletePaymentCardCommandHandler> _logger;

        public DeletePaymentCardCommandHandler(
           IPaymentCardRepository paymentCardRepository,
           IProductRepository productRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeletePaymentCardCommandHandler> logger
            )
        {
            _paymentCardRepository = paymentCardRepository;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeletePaymentCardCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _paymentCardRepository.FindAsync(request.Id);
            if (entityExist == null)
            {
                _logger.LogError("Payment Card Does not exists");
                return ServiceResponse<bool>.Return404("Payment Card Does not exists");
            }
            _paymentCardRepository.Delete(entityExist);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving Payment Card.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}