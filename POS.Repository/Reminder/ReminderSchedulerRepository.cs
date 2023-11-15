using POS.Common.GenericRepository;
using POS.Common.UnitOfWork;
using POS.Data;
using POS.Data.Dto;
using POS.Data.Entities;
using POS.Data.Resources;
using POS.Domain;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class ReminderSchedulerRepository : GenericRepository<ReminderScheduler, POSDbContext>, IReminderSchedulerRepository
    {
        private readonly IUnitOfWork<POSDbContext> _uow;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly UserInfoToken _userInfoToken;
        public ReminderSchedulerRepository(
            IUnitOfWork<POSDbContext> uow,
             IPropertyMappingService propertyMappingService,
               UserInfoToken userInfoToken
            ) : base(uow)
        {
            _uow = uow;
            _propertyMappingService = propertyMappingService;
            _userInfoToken = userInfoToken;
        }

        public async Task<bool> AddMultiReminder(List<Reminder> reminders)
        {
            if (reminders.Count() > 0)
            {
                var currentDate = DateTime.UtcNow;
                List<ReminderScheduler> lstReminderScheduler = new();
                foreach (var reminder in reminders)
                {
                    foreach (var reminderUser in reminder.ReminderUsers)
                    {
                        var reminderScheduler = new ReminderScheduler
                        {
                            Application = ApplicationEnums.Reminder,
                            ReferenceId = reminderUser.ReminderId,
                            Frequency = reminder.Frequency,
                            CreatedDate = DateTime.UtcNow,
                            IsActive = true,
                            Duration = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, reminder.StartDate.Hour, reminder.StartDate.Minute, reminder.StartDate.Second),
                            UserId = reminderUser.UserId,
                            IsEmailNotification = reminder.IsEmailNotification,
                            IsRead = false,
                            Subject = reminder.Subject,
                            Message = reminder.Message
                        };
                        lstReminderScheduler.Add(reminderScheduler);
                    }
                }
                AddRange(lstReminderScheduler);
                if (await _uow.SaveAsync() <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<PagedList<ReminderScheduler>> GetReminders(ReminderResource reminderResource)
        {
            var collectionBeforePaging =
              All.ApplySort(reminderResource.OrderBy,
              _propertyMappingService.GetPropertyMapping<ReminderSchedulerDto, ReminderScheduler>());

            collectionBeforePaging = collectionBeforePaging
                 .Where(c => c.UserId == Guid.Parse(_userInfoToken.Id));

            if (!string.IsNullOrWhiteSpace(reminderResource.Subject))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Subject, $"%{reminderResource.Subject}%"));
            }

            if (!string.IsNullOrWhiteSpace(reminderResource.Message))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(c => EF.Functions.Like(c.Message, $"%{reminderResource.Message}%"));
            }

            return await PagedList<ReminderScheduler>.Create(
                collectionBeforePaging,
                reminderResource.Skip,
                reminderResource.PageSize
                );
        }
        public async Task<bool> MarkAsRead()
        {
            await _uow.Context.Database.ExecuteSqlInterpolatedAsync($"Update ReminderSchedulers SET IsRead=1 where UserId={Guid.Parse(_userInfoToken.Id)};");
            return true;
        }
    }
}
