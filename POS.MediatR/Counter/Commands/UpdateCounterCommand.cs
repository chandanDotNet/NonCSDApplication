using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Counter.Commands
{
    public class UpdateCounterCommand : IRequest<ServiceResponse<CounterDto>>
    {
        public Guid Id { get; set; }
        public string CounterName { get; set; }
        public string EndPoint { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
    }
}
