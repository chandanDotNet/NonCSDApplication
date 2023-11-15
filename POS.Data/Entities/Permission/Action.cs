using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data
{
    public class Action : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public Guid PageId { get; set; }
        public Page Page { get; set; }
        public string Code { get; set; }
    }
}
