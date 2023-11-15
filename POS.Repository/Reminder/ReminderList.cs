using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class ReminderList : List<ReminderDto>
    {
        public ReminderList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public ReminderList(List<ReminderDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<ReminderList> Create(IQueryable<Reminder> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new ReminderList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Reminder> source)
        {
            return await source.AsNoTracking().CountAsync();
        }

        public async Task<List<ReminderDto>> GetDtos(IQueryable<Reminder> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(c => new ReminderDto
                {
                    Id = c.Id,
                    Message = c.Message,
                    Frequency = c.Frequency,
                    IsEmailNotification = c.IsEmailNotification,
                    IsRepeated = c.IsRepeated,
                    StartDate = c.StartDate,
                    EndDate= c.EndDate,
                    Subject = c.Subject,
                    CreatedDate = c.CreatedDate,
                }).ToListAsync();
            return entities;
        }
    }
}
