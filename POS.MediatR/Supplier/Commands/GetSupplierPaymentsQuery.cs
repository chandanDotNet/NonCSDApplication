using MediatR;
using POS.Data.Resources;
using POS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Supplier.Commands
{
    public class GetSupplierPaymentsQuery : IRequest<SupplierPaymentList>
    {
        public SupplierResource SupplierResource { get; set; }
    }
}
