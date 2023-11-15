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
    public class DeletePurchaseOrderPaymentCommandHandler : IRequestHandler<DeletePurchaseOrderPaymentCommand, ServiceResponse<bool>>
    {
        private readonly IPurchaseOrderPaymentRepository _purchaseOrderPaymentRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<DeletePurchaseOrderPaymentCommandHandler> _logger;

        public DeletePurchaseOrderPaymentCommandHandler(
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeletePurchaseOrderPaymentCommandHandler> logger,
            IPurchaseOrderPaymentRepository purchaseOrderPaymentRepository,
            IPurchaseOrderRepository purchaseOrderRepository)
        {
            _uow = uow;
            _logger = logger;
            _purchaseOrderPaymentRepository = purchaseOrderPaymentRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ServiceResponse<bool>> Handle(DeletePurchaseOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrderPayment = await _purchaseOrderPaymentRepository.FindAsync(request.Id);
            if (purchaseOrderPayment == null)
            {
                return ServiceResponse<bool>.Return404("Purchase Order payment not found.");
            }

            var purchaseOrder = await _purchaseOrderRepository.All.FirstOrDefaultAsync(c => c.Id == purchaseOrderPayment.PurchaseOrderId);
            _purchaseOrderPaymentRepository.Delete(purchaseOrderPayment);
            purchaseOrder.TotalPaidAmount = purchaseOrder.TotalPaidAmount - purchaseOrderPayment.Amount;

            if (purchaseOrder.TotalPaidAmount == 0)
            {
                purchaseOrder.PaymentStatus = PaymentStatus.Pending;
            }
            else if (purchaseOrder.TotalAmount <= purchaseOrder.TotalPaidAmount - purchaseOrderPayment.Amount)
            {
                purchaseOrder.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                purchaseOrder.PaymentStatus = PaymentStatus.Partial;
            }

            _purchaseOrderRepository.Update(purchaseOrder);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while deleting Purchase Order Payment.");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnSuccess();
        }
    }
}
