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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace POS.MediatR.PurchaseOrderPayment.Handler
{
    public class GetAllPurchaseOrderPaymentsCommandHanlder : IRequestHandler<GetAllPurchaseOrderPaymentsCommand, List<PurchaseOrderPaymentDto>>
    {
        private readonly IPurchaseOrderPaymentRepository _purchaseOrderPaymentRepository;
        private readonly IMapper _mapper;

        public GetAllPurchaseOrderPaymentsCommandHanlder(
            IMapper mapper,
            IPurchaseOrderPaymentRepository purchaseOrderPaymentRepository)
        {
            _mapper = mapper;
            _purchaseOrderPaymentRepository = purchaseOrderPaymentRepository;
        }

        public async Task<List<PurchaseOrderPaymentDto>> Handle(GetAllPurchaseOrderPaymentsCommand request, CancellationToken cancellationToken)
        {
            var payment = await _purchaseOrderPaymentRepository.All.Where(c => c.PurchaseOrderId == request.Id).ToListAsync();
            return _mapper.Map<List<PurchaseOrderPaymentDto>>(payment);
        }
    }
}
