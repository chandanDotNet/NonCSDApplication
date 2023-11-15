using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data
{
    public class UnitConversation : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Operator? Operator { get; set; }
        public decimal? Value { get; set; }
        public Guid? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public UnitConversation Parent { get; set; }
    }
}
