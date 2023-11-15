using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetReminderByIdQueryHandler : IRequestHandler<GetReminderByIdQuery, ServiceResponse<ReminderDto>>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IMapper _mapper;

        public GetReminderByIdQueryHandler(IReminderRepository reminderRepository,
            IMapper mapper)
        {
            _reminderRepository = reminderRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<ReminderDto>> Handle(GetReminderByIdQuery request, CancellationToken cancellationToken)
        {
            var reminder = await _reminderRepository
                .AllIncluding(cs => cs.ReminderUsers, c => c.DailyReminders, c => c.QuarterlyReminders, c => c.HalfYearlyReminders).FirstOrDefaultAsync(c => c.Id == request.Id);

            if (reminder == null)
                return ServiceResponse<ReminderDto>.Return404();

            return ServiceResponse<ReminderDto>.ReturnResultWith200(_mapper.Map<ReminderDto>(reminder));
        }
    }
}

