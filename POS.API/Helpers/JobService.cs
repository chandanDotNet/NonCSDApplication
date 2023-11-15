using POS.MediatR.CommandAndQuery;
using POS.Repository;
using Hangfire;
using MediatR;
using System;

namespace POS.API.Helpers
{
    public class JobService
    {
        public IMediator _mediator { get; set; }
        private readonly IConnectionMappingRepository _connectionMappingRepository;

        public JobService(IMediator mediator,
            IConnectionMappingRepository connectionMappingRepository)
        {
            _mediator = mediator;
            _connectionMappingRepository = connectionMappingRepository;
        }
        public void StartScheduler()
        {
            // * * * * *
            // 1 2 3 4 5

            // field #   meaning        allowed values
            // -------   ------------   --------------
            //    1      minute         0-59
            //    2      hour           0-23
            //    3      day of month   1-31
            //    4      month          1-12 (or use names)
            //    5      day of week    0-7 (0 or 7 is Sun, or use names)


            //Daily Reminder
            RecurringJob.AddOrUpdate(() => DailyReminder(), "0 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Weekly Reminder
            RecurringJob.AddOrUpdate(() => WeeklyReminder(), "10 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Monthy Reminder
            RecurringJob.AddOrUpdate(() => MonthyReminder(), "20 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Quarterly Reminder
            RecurringJob.AddOrUpdate(() => QuarterlyReminder(), "30 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //HalfYearly Reminder
            RecurringJob.AddOrUpdate(() => HalfYearlyReminder(), "40 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Yearly Reminder                                                                                
            RecurringJob.AddOrUpdate(() => YearlyReminder(), "50 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Customer Date
            RecurringJob.AddOrUpdate(() => CustomDateReminderSchedule(), "59 0 * * *", TimeZoneInfo.Utc); // Every 24 hours

            //Reminder Scheduler To Send Email
            RecurringJob.AddOrUpdate(() => ReminderSchedule(), "*/10 * * * *", TimeZoneInfo.Utc); // Every 10 minutes

            //Send Email Scheduler To Send Email
            RecurringJob.AddOrUpdate(() => SendEmailSuppliersSchedule(), "*/15 * * * *", TimeZoneInfo.Utc); // Every 10 minutes

            ////Expected Delivery For Purchase Order
            //RecurringJob.AddOrUpdate(() => ExpectedDeliveryForPurchaseOrder(), "5 0 * * *", TimeZoneInfo.Local); // Every 24 hours

            ////Expected Delivery For Sales Order
            //RecurringJob.AddOrUpdate(() => ExpectedDeliveryForSaleOrder(), "15 0 * * *", TimeZoneInfo.Local); // Every 24 hours
        }

        public bool DailyReminder()
        {
            return _mediator.Send(new DailyReminderServicesQuery()).GetAwaiter().GetResult();
        }
        public bool WeeklyReminder()
        {
            return _mediator.Send(new WeeklyReminderServicesQuery()).GetAwaiter().GetResult();

        }
        public bool MonthyReminder()
        {
            return _mediator.Send(new MonthlyReminderServicesQuery()).GetAwaiter().GetResult();
        }
        public bool QuarterlyReminder()
        {
            return _mediator.Send(new QuarterlyReminderServiceQuery()).GetAwaiter().GetResult();
        }

        public bool HalfYearlyReminder()
        {
            return _mediator.Send(new HalfYearlyReminderServiceQuery()).GetAwaiter().GetResult();
        }

        public bool YearlyReminder()
        {
            return _mediator.Send(new YearlyReminderServicesQuery()).GetAwaiter().GetResult();
        }

        public bool ReminderSchedule()
        {
            var schedulerStatus = _connectionMappingRepository.GetSchedulerServiceStatus();
            if (!schedulerStatus)
            {
                _connectionMappingRepository.SetSchedulerServiceStatus(true);
                var result = _mediator.Send(new ReminderSchedulerServiceQuery()).GetAwaiter().GetResult();
                _connectionMappingRepository.SetSchedulerServiceStatus(false);
                return result;
            }
            return true;
        }

        public bool CustomDateReminderSchedule()
        {
            return _mediator.Send(new CustomDateReminderServicesQuery()).GetAwaiter().GetResult();
        }

        public bool SendEmailSuppliersSchedule()
        {
            var schedulerStatus = _connectionMappingRepository.GetEmailSchedulerStatus();
            if (!schedulerStatus)
            {
                _connectionMappingRepository.SetEmailSchedulerStatus(true);
                var result = _mediator.Send(new SendEmailSchedulerSupplierCommand()).GetAwaiter().GetResult();
                _connectionMappingRepository.SetEmailSchedulerStatus(false);
                return result;
            }
            return true;
        }
    }
}
