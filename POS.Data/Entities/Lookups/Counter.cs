using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Counter : BaseEntity
    {
        public Guid Id { get; set; }
        public string CounterName { get; set; }
    }
}
