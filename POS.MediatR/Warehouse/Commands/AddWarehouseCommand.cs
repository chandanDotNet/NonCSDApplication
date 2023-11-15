using MediatR;
using POS.Data.Dto;
using POS.Helper;

namespace POS.MediatR.Warehouse.Commands
{
    public class AddWarehouseCommand : IRequest<ServiceResponse<WarehouseDto>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }
}
