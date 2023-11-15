using AutoMapper;
using POS.Common.UnitOfWork;
using POS.Data;
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
    public class UpdateExpenseCategoryCommandHanlder
        : IRequestHandler<UpdateExpenseCategoryCommand, ServiceResponse<bool>>
    {
        private readonly IExpenseCategoryRepository _expenseCategoryRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly ILogger<UpdateExpenseCategoryCommandHanlder> _logger;
        private readonly IMapper _mapper;
        public UpdateExpenseCategoryCommandHanlder(
           IExpenseCategoryRepository expenseCategoryRepository,
            IUnitOfWork<POSDbContext> uow,
            ILogger<UpdateExpenseCategoryCommandHanlder> logger,
            IMapper mapper
            )
        {
            _expenseCategoryRepository = expenseCategoryRepository;
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<bool>> Handle(UpdateExpenseCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingEntity = await _expenseCategoryRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id).FirstOrDefaultAsync();
            if (existingEntity != null)
            {
                _logger.LogError("Expense Category already Exists.");
                return ServiceResponse<bool>.Return409("Expense Category already Exists.");
            }
            existingEntity= await _expenseCategoryRepository.FindAsync(request.Id);

            if (existingEntity == null)
            {
                _logger.LogError("Expense Category does not Exists.");
                return ServiceResponse<bool>.Return409("Expense Category does not Exists.");
            }
            existingEntity.Name = request.Name;
            _expenseCategoryRepository.Update(existingEntity);
            if (await _uow.SaveAsync() <= 0)
            {

                _logger.LogError("Error while saving Expense Category");
                return ServiceResponse<bool>.Return500();
            }
            return ServiceResponse<bool>.ReturnResultWith200(true);
        }
    }
}
