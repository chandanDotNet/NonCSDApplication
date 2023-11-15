using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System.Collections.Generic;

namespace POS.MediatR.Product.Command
{
    public class GetProductsDetailCommand : IRequest<ServiceResponse<List<ProductDto>>>
    {

    }
}
