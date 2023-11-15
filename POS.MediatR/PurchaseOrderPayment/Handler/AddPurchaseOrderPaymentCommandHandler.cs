using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using POS.MediatR.PurchaseOrderPayment.Command;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.PurchaseOrderPayment.Handler
{

    public class AddPurchaseOrderPaymentCommandHandler : IRequestHandler<AddPurchaseOrderPaymentCommand, ServiceResponse<PurchaseOrderPaymentDto>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IPurchaseOrderPaymentRepository _purchaseOrderPaymentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPurchaseOrderPaymentCommandHandler> _logger;

        public AddPurchaseOrderPaymentCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddPurchaseOrderPaymentCommandHandler> logger,
            IPurchaseOrderPaymentRepository purchaseOrderPaymentRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _purchaseOrderPaymentRepository = purchaseOrderPaymentRepository;
        }

        public async Task<ServiceResponse<PurchaseOrderPaymentDto>> Handle(AddPurchaseOrderPaymentCommand request, CancellationToken cancellationToken)
        {

            var purchaseOrder = await _purchaseOrderRepository.All.FirstOrDefaultAsync(c => c.Id == request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return ServiceResponse<PurchaseOrderPaymentDto>.Return404("Purchase Order not found.");
            }

            var purchaseOrderPayment = _mapper.Map<Data.PurchaseOrderPayment>(request);
            _purchaseOrderPaymentRepository.Add(purchaseOrderPayment);

            if (purchaseOrder.TotalAmount <= (purchaseOrderPayment.Amount + purchaseOrder.TotalPaidAmount))
            {
                purchaseOrder.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                purchaseOrder.PaymentStatus = PaymentStatus.Partial;
            }
            purchaseOrder.TotalPaidAmount += purchaseOrderPayment.Amount;
            _purchaseOrderRepository.Update(purchaseOrder);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order Payment.");
                return ServiceResponse<PurchaseOrderPaymentDto>.Return500();
            }

            var dto = _mapper.Map<PurchaseOrderPaymentDto>(purchaseOrderPayment);
            return ServiceResponse<PurchaseOrderPaymentDto>.ReturnResultWith201(dto);
        }
    }
}
