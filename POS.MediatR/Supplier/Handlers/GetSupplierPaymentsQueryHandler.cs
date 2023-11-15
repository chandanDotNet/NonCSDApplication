using MediatR;
using POS.MediatR.Supplier.Commands;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Supplier.Handlers
{
    public class GetSupplierPaymentsQueryHandler
        : IRequestHandler<GetSupplierPaymentsQuery, SupplierPaymentList>
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetSupplierPaymentsQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<SupplierPaymentList> Handle(GetSupplierPaymentsQuery request, CancellationToken cancellationToken)
        {
            return await _supplierRepository.GetSuppliersPayment(request.SupplierResource);
        }
    }
}
