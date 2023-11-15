using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Domain;
using POS.Helper;
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
    public class AddExpenseCategoryCommandHandler
        : IRequestHandler<AddExpenseCategoryCommand, ServiceResponse<ExpenseCategoryDto>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AddExpenseCategoryCommandHandler> _logger;
        public AddExpenseCategoryCommandHandler(
           IExpenseCategoryRepository expenseCategoryRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<AddExpenseCategoryCommandHandler> logger
            )
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }
        public async Task<ServiceResponse<ExpenseCategoryDto>> Handle(AddExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _expenseCategoryRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Expence Category Already Exist");
                return ServiceResponse<ExpenseCategoryDto>.Return409("Expence Category Already Exist.");
            }
            var entity = _mapper.Map<ExpenseCategory>(request);
            entity.Id = Guid.NewGuid();
            _expenseCategoryRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error While saving Expence Category.");
                return ServiceResponse<ExpenseCategoryDto>.Return500();
            }
            return ServiceResponse<ExpenseCategoryDto>.ReturnResultWith200(_mapper.Map<ExpenseCategoryDto>(entity));
        }
    }
}
