using System;

namespace POS.Data.Dto
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
