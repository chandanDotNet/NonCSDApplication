using POS.Data.Dto;
using POS.Data.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CommandAndQuery
{
    public class GetAllReminderSchedulerQuery : IRequest<List<ReminderSchedulerDto>>
    {
        public ApplicationEnums Application { get; set; }
        public Guid ReferenceId { get; set; }
    }
}
