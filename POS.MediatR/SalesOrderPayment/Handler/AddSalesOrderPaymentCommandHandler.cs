using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.SalesOrderPayment.Command;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.SalesOrderPayment.Handler
{
    public class AddSalesOrderPaymentCommandHandler : IRequestHandler<AddSalesOrderPaymentCommand, ServiceResponse<SalesOrderPaymentDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderPaymentRepository _salesOrderPaymentRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddSalesOrderPaymentCommandHandler> _logger;

        public AddSalesOrderPaymentCommandHandler(
            ISalesOrderRepository salesOrderRepository,
            IUnitOfWork<POSDbContext> uow,
            IMapper mapper,
            ILogger<AddSalesOrderPaymentCommandHandler> logger,
            ISalesOrderPaymentRepository salesOrderPaymentRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _salesOrderPaymentRepository = salesOrderPaymentRepository;
        }
        

        public async Task<ServiceResponse<SalesOrderPaymentDto>> Handle(AddSalesOrderPaymentCommand request, CancellationToken cancellationToken)
        {

            var salesOrder = await _salesOrderRepository.All.FirstOrDefaultAsync(c => c.Id == request.SalesOrderId);
            if (salesOrder == null)
            {
                return ServiceResponse<SalesOrderPaymentDto>.Return404("Sales Order not found.");
            }

            var salesOrderPayment = _mapper.Map<Data.SalesOrderPayment>(request);
            _salesOrderPaymentRepository.Add(salesOrderPayment);

            if (salesOrder.TotalAmount <= (salesOrderPayment.Amount + salesOrder.TotalPaidAmount))
            {
                salesOrder.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                salesOrder.PaymentStatus = PaymentStatus.Partial;
            }
            salesOrder.TotalPaidAmount += salesOrderPayment.Amount;
            _salesOrderRepository.Update(salesOrder);
            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error while creating Purchase Order Payment.");
                return ServiceResponse<SalesOrderPaymentDto>.Return500();
            }

            var dto = _mapper.Map<SalesOrderPaymentDto>(salesOrderPayment);
            return ServiceResponse<SalesOrderPaymentDto>.ReturnResultWith201(dto);
        }
    }
}
