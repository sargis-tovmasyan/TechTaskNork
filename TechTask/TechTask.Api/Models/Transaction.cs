namespace TechTask.Api.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
