using AutoMapper;
using POS.Data;
using POS.Data.Dto;
using POS.MediatR.CommandAndQuery;
using System;

namespace POS.API.Helpers.Mapping
{
    public class CompanyProfileProfile : Profile
    {
        public CompanyProfileProfile()
        {
            CreateMap<CompanyProfile, CompanyProfileDto>().ReverseMap();
            CreateMap<UpdateCompanyProfileCommand, CompanyProfile>();
        }
    }
}
