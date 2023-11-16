﻿using MediatR;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.CustomerAddress.Commands
{
    public class DeleteCustomerAddressCommand : IRequest<ServiceResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}