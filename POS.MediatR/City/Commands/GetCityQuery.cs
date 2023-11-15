using MediatR;
using POS.Data.Dto;
using POS.Data.Resources;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.City.Command
{
    public class GetCityQuery : IRequest<ServiceResponse<CityDto>>
    {
        public Guid Id { get; set; }
    }

}
