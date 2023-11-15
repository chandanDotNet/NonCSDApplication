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
    public class AddProductCategoryCommand 
        : IRequest<ServiceResponse<ProductCategoryDto>>
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Description { get; set; }
    }
}
