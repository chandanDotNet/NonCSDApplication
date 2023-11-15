using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetExpenseQueryHandler
        : IRequestHandler<GetExpenseQuery, ServiceResponse<ExpenseDto>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetExpenseQueryHandler> _logger;
        public GetExpenseQueryHandler(IExpenseRepository expenseRepository,
            IMapper mapper,
            ILogger<GetExpenseQueryHandler> logger)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ServiceResponse<ExpenseDto>> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
        {
            var expense = await _expenseRepository.FindAsync(request.Id);
            if (expense == null)
            {
                _logger.LogError("Expense not found");
                return ServiceResponse<ExpenseDto>.Return404();
            }
            var expenseDto = _mapper.Map<ExpenseDto>(expense);
            return ServiceResponse<ExpenseDto>.ReturnResultWith200(expenseDto);
        }
    }
}
