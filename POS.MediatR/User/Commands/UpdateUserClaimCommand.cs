using POS.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class UpdateUserClaimCommand : IRequest<ServiceResponse<UserClaimDto>>
    {
        public Guid Id { get; set; }
        public List<UserClaimDto> UserClaims { get; set; } = new List<UserClaimDto>();
    }
}
