using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, ServiceResponse<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSupplierQueryHandler> _logger;
        private readonly PathHelper _pathHelper;
        public GetSupplierQueryHandler(
           ISupplierRepository supplierRepository,
            IMapper mapper,
            ILogger<GetSupplierQueryHandler> logger,
             PathHelper pathHelper
            )
        {

            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _logger = logger;
            _pathHelper = pathHelper;
        }

        public async Task<ServiceResponse<SupplierDto>> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var entity = await _supplierRepository.AllIncluding(c => c.SupplierAddress, b => b.BillingAddress, s => s.ShippingAddress)
                .Where(s => s.Id == request.Id).FirstOrDefaultAsync();
            if (entity != null)
            {
                var entityDto = _mapper.Map<SupplierDto>(entity);
                entityDto.ImageUrl = string.IsNullOrWhiteSpace(entityDto.Url) ? ""
                    : Path.Combine(_pathHelper.SupplierImagePath, entityDto.Url);
                return ServiceResponse<SupplierDto>.ReturnResultWith200(entityDto);
            }
            else
            {
                _logger.LogError("User not found");
                return ServiceResponse<SupplierDto>.ReturnFailed(404, "User not found");
            }
        }


    }
}
