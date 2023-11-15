using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetExpenseCategoryQueryHandler
       : IRequestHandler<GetExpenseCategoryQuery, ServiceResponse<ExpenseCategoryDto>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetExpenseCategoryQueryHandler> _logger;

        public GetExpenseCategoryQueryHandler(
            IExpenseCategoryRepository expenseCategoryRepository,
            IMapper mapper,
            ILogger<GetExpenseCategoryQueryHandler> logger)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ServiceResponse<ExpenseCategoryDto>> Handle(GetExpenseCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _expenseCategoryRepository.FindAsync(request.Id);
            if (entity != null)
                return ServiceResponse<ExpenseCategoryDto>.ReturnResultWith200(_mapper.Map<ExpenseCategoryDto>(entity));
            else
            {
                _logger.LogError("Not found");
                return ServiceResponse<ExpenseCategoryDto>.Return404();
            }
        }
    }
}
