using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetSalesOrderQuaryHandler : IRequestHandler<GetSalesOrderCommand, ServiceResponse<SalesOrderDto>>
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IMapper _mapper;

        public GetSalesOrderQuaryHandler(ISalesOrderRepository salesOrderRepository,
            IMapper mapper)
        {
            _salesOrderRepository = salesOrderRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<SalesOrderDto>> Handle(GetSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _salesOrderRepository.All
                 .Include(c => c.SalesOrderPayments)
                 .Include(c => c.Customer)
                 .Include(c => c.SalesOrderItems)
                     .ThenInclude(c => c.SalesOrderItemTaxes)
                     .ThenInclude(cs => cs.Tax)
                .Include(c => c.SalesOrderItems)
                    .ThenInclude(c => c.Product)
                .Include(c => c.SalesOrderItems)
                    .ThenInclude(c => c.UnitConversation)
                .Where(c => c.Id == request.Id)
                .FirstOrDefaultAsync();
            if (entity == null)
            {
                return ServiceResponse<SalesOrderDto>.Return404();
            }
            var dto = _mapper.Map<SalesOrderDto>(entity);
            return ServiceResponse<SalesOrderDto>.ReturnResultWith200(dto);
        }
    }
}
