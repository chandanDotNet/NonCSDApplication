using AutoMapper;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllExpenseCategoryQueryHandler
        : IRequestHandler<GetAllExpenseCategoryQuery, List<ExpenseCategoryDto>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IMapper _mapper;

        public GetAllExpenseCategoryQueryHandler(
            IExpenseCategoryRepository expenseCategoryRepository,
            IMapper mapper)
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _mapper = mapper;
        }

        public async Task<List<ExpenseCategoryDto>> Handle(GetAllExpenseCategoryQuery request, CancellationToken cancellationToken)
        {
            var entities = await _expenseCategoryRepository.All.ToListAsync();
            var dtoEntities = _mapper.Map<List<ExpenseCategoryDto>>(entities);
            return dtoEntities;
        }
    }
}
