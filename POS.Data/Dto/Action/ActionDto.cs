using System;

namespace POS.Data.Dto
{
    public class ActionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PageId { get; set; }
        public string Code { get; set; }
        public int Order { get; set; }
    }
}
