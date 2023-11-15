using POS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Resources
{
    public class CityResource : ResourceParameters
    {
        public CityResource() : base("CityName")
        {

        }
        public string CountryName { get; set; }
        public string CityName { get; set; }
    }
}
