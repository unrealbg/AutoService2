namespace AutoService2.Models
{
    // Модел за поръчка/ремонт
    public class WorkOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public WorkOrderStatus Status { get; set; }
        public WorkOrderPriority Priority { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TotalCost => LaborCost + PartsCost;
        public List<WorkOrderItem> Items { get; set; } = new();
        public int? InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
    }
}
