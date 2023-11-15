using POS.Data.Dto;
using MediatR;
using System;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateActionCommand : IRequest<ServiceResponse<ActionDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PageId { get; set; }
        public int Order { get; set; }
    }
}
