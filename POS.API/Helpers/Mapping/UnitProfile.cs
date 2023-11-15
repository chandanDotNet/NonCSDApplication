using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.UnitConversation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Helpers.Mapping
{
    public class UnitProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnitProfile()
        {

            CreateMap<UnitConversation, UnitConversationDto>().ReverseMap();
            CreateMap<AddUnitConversationCommand, UnitConversation>();
            CreateMap<UpdateUnitConversationCommand, UnitConversation>();
        }
    }
}
