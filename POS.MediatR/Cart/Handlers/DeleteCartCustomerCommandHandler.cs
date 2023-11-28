using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POS.Common.UnitOfWork;
using POS.Domain;
using POS.Helper;
using POS.MediatR.Cart.Commands;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Cart.Handlers
{
    public class DeleteCartCustomerCommandHandler : IRequestHandler<DeleteCartByCustomerCommand, ServiceResponse<bool>>
    {

        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCartCommandHandler> _logger;
        public DeleteCartCustomerCommandHandler(
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

        public async Task<ServiceResponse<bool>> Handle(DeleteCartByCustomerCommand request, CancellationToken cancellationToken)
        {
            Guid guid = request.CustomerId;
            
           // _cartRepository.Delete(existingCart);
            var result = _cartRepository.All.Where(c => c.CustomerId == request.CustomerId).ToList();
            if(result.Count>0)
            {
                foreach (var cartdata in result)
                {
                    var existingCart = await _cartRepository.FindAsync(cartdata.Id);
                    _cartRepository.Delete(existingCart);
                    if (await _uow.SaveAsync() <= 0)
                    {
                        _logger.LogError("Error While deleting Cart.");
                        return ServiceResponse<bool>.Return500();
                    }
                }
            }
            //_cartRepository.Delete(result.FirstOrDefault());
            //var existingCart = await _cartRepository.All.Any(c => c.CustomerId = request.CustomerId);
            //if (existingCart == null)
            //{
            //    _logger.LogError("Data does not exists.");
            //    return ServiceResponse<bool>.Return404("Data does not exists.");
            //}

            //_cartRepository.Delete(existingCart);

            //if (await _uow.SaveAsync() <= 0)
            //{
            //    _logger.LogError("Error While deleting Cart.");
            //    return ServiceResponse<bool>.Return500();
            //}

            return ServiceResponse<bool>.ReturnSuccess();
        }

    }
}
