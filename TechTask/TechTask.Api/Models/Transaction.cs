using System.ComponentModel.DataAnnotations;

namespace TechTask.Api.Models
{
    public enum TransactionType
    {
        Purchase, // + stock quantity
        Sale      // - stock quantity
    }

    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total amount cannot be negative.")]
        public double TotalAmount { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [EnumDataType(typeof(TransactionType), ErrorMessage = "Invalid transaction type.")]
        public TransactionType Type { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        public Product Product { get; set; } 
    }
}
