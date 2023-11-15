using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Country : BaseEntity
    {
        public Guid Id { get; set; }
        public string CountryName { get; set; }
    }
}
