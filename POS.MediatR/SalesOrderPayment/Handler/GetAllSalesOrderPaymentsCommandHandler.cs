using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
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
    public class GetAllSalesOrderPaymentsCommandHandler : IRequestHandler<GetAllSalesOrderPaymentsCommand, List<SalesOrderPaymentDto>>
    {
        private readonly ISalesOrderPaymentRepository _salesOrderPaymentRepository;
        private readonly IMapper _mapper;

        public GetAllSalesOrderPaymentsCommandHandler(
            IMapper mapper,
            ISalesOrderPaymentRepository salesOrderPaymentRepository)
        {
            _mapper = mapper;
            _salesOrderPaymentRepository = salesOrderPaymentRepository;
        }

        public async Task<List<SalesOrderPaymentDto>> Handle(GetAllSalesOrderPaymentsCommand request, CancellationToken cancellationToken)
        {
            var payment = await _salesOrderPaymentRepository.All.Where(c => c.SalesOrderId == request.Id).ToListAsync();
            return _mapper.Map<List<SalesOrderPaymentDto>>(payment);
        }
    }
}
