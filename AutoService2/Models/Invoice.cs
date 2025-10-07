namespace AutoService2.Models
{
    // Модел за фактура
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }
        public decimal Subtotal { get; set; }
        public decimal VATRate { get; set; }
        public decimal VATAmount => Subtotal * VATRate;
        public decimal Total => Subtotal + VATAmount;
        public InvoiceStatus Status { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
