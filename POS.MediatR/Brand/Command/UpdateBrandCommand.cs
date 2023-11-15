using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Brand.Command
{
    public class UpdateBrandCommand : IRequest<ServiceResponse<BrandDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrlData { get; set; }
        public bool IsImageChanged { get; set; }
    }
}
