using POS.Data.Dto;
using MediatR;
using System;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class GetPageQuery : IRequest<ServiceResponse<PageDto>>
    {
        public Guid Id { get; set; }
    }
}
