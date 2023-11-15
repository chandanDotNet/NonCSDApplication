using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.Tax.Commands;

namespace POS.API.Helpers.Mapping
{
    public class TaxProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TaxProfile()
        {
            CreateMap<Tax, TaxDto>().ReverseMap();
            CreateMap<AddTaxCommand, Tax>();
            CreateMap<UpdateTaxCommand, Tax>();
        }
    }
}
