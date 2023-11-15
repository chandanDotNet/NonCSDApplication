
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Data.Dto {
  public class ExpenseDto {
    public Guid Id { get; set; }
    public string Reference { get; set; }
    public Guid ExpenseCategoryId { get; set; }
    public ExpenseCategoryDto ExpenseCategory { get; set; }
    public decimal Amount { get; set; }
    public Guid? ExpenseById { get; set; }
    public string Description { get; set; }
    public UserDto ExpenseBy { get; set; }
    public DateTime ExpenseDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ReceiptName { get; set; }
  }
}
