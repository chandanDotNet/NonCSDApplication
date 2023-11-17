using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
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
    public class AddPaymentCardCommandHandler :
        IRequestHandler<AddPaymentCardCommand, ServiceResponse<PaymentCardDto>>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPaymentCardCommandHandler> _logger;
        public AddPaymentCardCommandHandler(
           IPaymentCardRepository paymentCardRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddPaymentCardCommandHandler> logger
            )
        {
            _paymentCardRepository = paymentCardRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<PaymentCardDto>> Handle(AddPaymentCardCommand request, CancellationToken cancellationToken)
        {
            
            var existingEntity = await _paymentCardRepository
                .All
                .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber);

            if (existingEntity != null)
            {
                _logger.LogError("Payment Card Already Exist");
                return ServiceResponse<PaymentCardDto>.Return409("Payment Card Already Exist.");
            }
            var entity = _mapper.Map<POS.Data.PaymentCard>(request);
            _paymentCardRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While saving IPayment Card.");
                return ServiceResponse<PaymentCardDto>.Return500();
            }
            return ServiceResponse<PaymentCardDto>.ReturnResultWith200(_mapper.Map<PaymentCardDto>(entity));
        }
    }
}

