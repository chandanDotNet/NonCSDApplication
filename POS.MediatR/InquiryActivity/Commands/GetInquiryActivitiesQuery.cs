using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;

namespace POS.MediatR.CommandAndQuery
{
    public class GetInquiryActivitiesQuery : IRequest<List<InquiryActivityDto>>
    {
        public Guid InquiryId { get; set; }
    }
}
