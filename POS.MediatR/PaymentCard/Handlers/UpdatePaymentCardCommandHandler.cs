using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
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
    public class UpdatePaymentCardCommandHandler : IRequestHandler<UpdatePaymentCardCommand, ServiceResponse<PaymentCardDto>>
    {
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdatePaymentCardCommandHandler> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public UpdatePaymentCardCommandHandler(
           IPaymentCardRepository paymentCardRepository,
           IUnitOfWork<POSDbContext> uow,
           ILogger<UpdatePaymentCardCommandHandler> logger,
           IWebHostEnvironment webHostEnvironment,
        IMapper mapper
           )
        {
            _paymentCardRepository = paymentCardRepository;
            _uow = uow;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<PaymentCardDto>> Handle(UpdatePaymentCardCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _paymentCardRepository.FindBy(c => c.CardNumber == request.CardNumber && c.Id != request.Id)
             .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                _logger.LogError("Payment Card Already Exist.");
                return ServiceResponse<PaymentCardDto>.Return409("Payment Card Already Exist.");
            }
            entityExist = await _paymentCardRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entityExist.CardNumber = request.CardNumber;
            entityExist.ExpiryDate = request.ExpiryDate;
            entityExist.CVV = request.CVV;
            entityExist.CardType = request.CardType;
            entityExist.NameOnCard = request.NameOnCard;
            entityExist.NickNameForCard = request.NickNameForCard;
            entityExist.IsPrimary = request.IsPrimary;

            _paymentCardRepository.Update(entityExist);

            //remove other as Primary Card
            if (entityExist.IsPrimary)
            {
                var defaultPrimaryCardPSettings = await _paymentCardRepository.All.Where(c => c.CustomerId == request.CustomerId && c.Id != request.Id && c.IsPrimary).ToListAsync();
                defaultPrimaryCardPSettings.ForEach(c => c.IsPrimary = false);
                _paymentCardRepository.UpdateRange(defaultPrimaryCardPSettings);
            }

            if (await _uow.SaveAsync() <= 0)
            {
                return ServiceResponse<PaymentCardDto>.Return500();
            }

            var result = _mapper.Map<PaymentCardDto>(entityExist);

            return ServiceResponse<PaymentCardDto>.ReturnResultWith200(result);
        }
    }
}