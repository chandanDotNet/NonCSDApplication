using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Dto
{
    public class UnitConversationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Operator? Operator { get; set; }
        public decimal? Value { get; set; }
        public Guid? ParentId { get; set; }
        public string BaseUnitName { get; set; }
        
    }
}
