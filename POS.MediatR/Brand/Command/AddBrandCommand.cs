using MediatR;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.Brand.Command
{
    public class AddBrandCommand : IRequest<ServiceResponse<BrandDto>>
    {
        public string Name { get; set; }
        public string ImageUrlData { get; set; }
    }
}
