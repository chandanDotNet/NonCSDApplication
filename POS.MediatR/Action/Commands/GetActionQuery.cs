﻿using POS.Data.Dto;
using MediatR;
using System;
using POS.Helper;

namespace POS.MediatR.CommandAndQuery
{
    public class GetActionQuery : IRequest<ServiceResponse<ActionDto>>
    {
        public Guid Id { get; set; }
    }
}
