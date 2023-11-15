using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class City : BaseEntity
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
    }
}
