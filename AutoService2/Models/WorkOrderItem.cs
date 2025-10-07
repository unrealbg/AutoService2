namespace AutoService2.Models
{
    // Позиция в поръчка
    public class WorkOrderItem
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal LaborHours { get; set; }
        public decimal HourlyRate { get; set; }
        public int? PartId { get; set; }
        public Part? Part { get; set; }
        public decimal PartPrice { get; set; }
        public decimal Total => (LaborHours * HourlyRate) + (Quantity * PartPrice);
    }
}
