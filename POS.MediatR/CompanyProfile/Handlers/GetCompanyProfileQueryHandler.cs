using AutoMapper;
using POS.Data.Dto;
using POS.Helper;
using POS.MediatR.CommandAndQuery;
using POS.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POS.MediatR.Handlers
{
    public class GetCompanyProfileQueryHandler
        : IRequestHandler<GetCompanyProfileQuery, CompanyProfileDto>
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly IMapper _mapper;
        private readonly PathHelper _pathHelper;

        public GetCompanyProfileQueryHandler(ICompanyProfileRepository companyProfileRepository,
            IMapper mapper,
            PathHelper pathHelper)
        {
            _companyProfileRepository = companyProfileRepository;
            _mapper = mapper;
            _pathHelper = pathHelper;
        }
        public async Task<CompanyProfileDto> Handle(GetCompanyProfileQuery request, CancellationToken cancellationToken)
        {
            var companyProfile = await _companyProfileRepository.All.FirstOrDefaultAsync();
            if (companyProfile == null)
            {
                return new CompanyProfileDto
                {
                    Address = "3822 Crim Lane Dayton, OH 45407",
                    LogoUrl = "",
                    Title = "Point of Sale",
                    CurrencyCode = "USD"
                };
            }
            if (!string.IsNullOrWhiteSpace(companyProfile.LogoUrl))
            {
                companyProfile.LogoUrl = Path.Combine(_pathHelper.CompanyLogo, companyProfile.LogoUrl);
            }
            return _mapper.Map<CompanyProfileDto>(companyProfile);
        }
    }
}
