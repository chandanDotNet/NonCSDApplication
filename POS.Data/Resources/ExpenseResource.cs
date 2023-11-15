using POS.Helper;
using System;

namespace POS.Data.Resources
{
    public class ExpenseResource : ResourceParameters
    {
        public ExpenseResource() : base("CreatedDate")
        {
        }
        public string Reference { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public Guid? ExpenseById { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
 
    }
}
