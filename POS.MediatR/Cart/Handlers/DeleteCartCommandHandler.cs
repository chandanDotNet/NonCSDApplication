using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.MediatR.Product.Command;
using POS.MediatR.Product.Handler;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Cart
{
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ServiceResponse<bool>>
    {

        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCartCommandHandler> _logger;
        public DeleteCartCommandHandler(
           ICartRepository cartRepository,
            IMapper mapper,
            IUnitOfWork<POSDbContext> uow,
            ILogger<DeleteCartCommandHandler> logger
            )
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ServiceResponse<bool>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var existingCart = await _cartRepository.FindAsync(request.Id);
            if (existingCart == null)
            {
                _logger.LogError("Data does not exists.");
                return ServiceResponse<bool>.Return404("Data does not exists.");
            }

            _cartRepository.Delete(existingCart);

            if (await _uow.SaveAsync() <= 0)
            {
                _logger.LogError("Error While deleting Cart.");
                return ServiceResponse<bool>.Return500();
            }

            return ServiceResponse<bool>.ReturnSuccess();
        }

    }
}
