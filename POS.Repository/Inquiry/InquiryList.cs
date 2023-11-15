using POS.Data;
using POS.Data.Dto;
using POS.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository
{
    public class InquiryList : List<InquiryShortDto>
    {
        public InquiryList()
        {
        }

        public int Skip { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public InquiryList(List<InquiryShortDto> items, int count, int skip, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            Skip = skip;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public async Task<InquiryList> Create(IQueryable<Inquiry> source, int skip, int pageSize)
        {
            var count = await GetCount(source);
            var dtoList = await GetDtos(source, skip, pageSize);
            var dtoPageList = new InquiryList(dtoList, count, skip, pageSize);
            return dtoPageList;
        }

        public async Task<int> GetCount(IQueryable<Inquiry> source)
        {
            try
            {
                return await source.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<InquiryShortDto>> GetDtos(IQueryable<Inquiry> source, int skip, int pageSize)
        {
            var entities = await source
                .Skip(skip)
                .Take(pageSize)
                .AsNoTracking()
                .Select(cs => new InquiryShortDto
                {
                    Id = cs.Id,
                    CompanyName = cs.CompanyName,
                    ContactPerson = cs.CompanyName,
                    Email = cs.Email,
                    Phone = cs.Phone,
                    MobileNo = cs.MobileNo,
                    Message = cs.Message,
                    CityName = cs.CityName,
                    CountryName = cs.CountryName,
                    InquiryStatus = cs.InquiryStatusId != null ? cs.InquiryStatus.Name : "",
                    InquirySource = cs.InquirySource.Name,
                    AssignToName = cs.AssignTo != null ? cs.AssignUser.FirstName + " " + cs.AssignUser.LastName : "",
                    CreatedDate = cs.CreatedDate,
                    InquiryNoteCount = cs.InquiryNotes.Count(),
                    InquiryAttachmentCount = cs.InquiryAttachments.Count(),
                    InquiryActivityCount = cs.InquiryActivities.Count()
                })
                .ToListAsync();
            return entities;
        }
    }
}
