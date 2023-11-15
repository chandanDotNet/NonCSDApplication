using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetAllSupplierQueryHandler : IRequestHandler<GetAllSupplierQuery, SupplierList>
    {
        private readonly ISupplierRepository _supplierRepository;
        public GetAllSupplierQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<SupplierList> Handle(GetAllSupplierQuery request, CancellationToken cancellationToken)
        {
            return await _supplierRepository.GetSuppliers(request.SupplierResource);
        }
    }
}
