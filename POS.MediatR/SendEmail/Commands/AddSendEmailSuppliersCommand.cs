using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class AddSendEmailSuppliersCommand : IRequest<bool>
    {
        public List<Guid> Suppliers { get; set; } = new List<Guid>();
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
