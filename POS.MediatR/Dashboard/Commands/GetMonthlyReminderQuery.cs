using POS.Data.Dto;
using POS.Helper;
using MediatR;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetMonthlyReminderQuery : IRequest<List<CalenderReminderDto>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
