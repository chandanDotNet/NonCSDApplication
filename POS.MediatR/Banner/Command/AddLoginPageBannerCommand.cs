using MediatR;
using POS.Data.Dto;
using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MediatR.Banner.Command
{
    public class AddLoginPageBannerCommand : IRequest<ServiceResponse<LoginPageBannerDto>>
    {
        public string Name { get; set; }
        public string ImageUrlData { get; set; }
    }
}
