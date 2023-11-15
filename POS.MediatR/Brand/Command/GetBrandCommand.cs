using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;

namespace POS.MediatR.Brand.Command
{
    public class GetBrandCommand : IRequest<ServiceResponse<BrandDto>>
    {
        public Guid Id { get; set; }
    }
}
