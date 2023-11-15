using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Category.Commands
{
    public class GetProductCategoryByIdQuery : IRequest<ServiceResponse<ProductCategoryDto>>
    {
        public Guid Id { get; set; }
    }
}
