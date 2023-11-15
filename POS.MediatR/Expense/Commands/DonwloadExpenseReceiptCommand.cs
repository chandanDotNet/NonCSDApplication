using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class DonwloadExpenseReceiptCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
