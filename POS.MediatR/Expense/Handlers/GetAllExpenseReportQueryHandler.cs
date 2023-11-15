using AutoMapper;
using MediatR;
using POS.MediatR.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllExpenseReportQueryHandler : IRequestHandler<GetAllExpenseReportQuery, ExpenseList>
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public GetAllExpenseReportQueryHandler(
            IExpenseRepository expenseRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<ExpenseList> Handle(GetAllExpenseReportQuery request, CancellationToken cancellationToken)
        {
            return await _expenseRepository.GetExpensesReport(request.ExpenseResource);
        }
    }
}