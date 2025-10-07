namespace AutoService2.Models
{
    // Модел за склад
    public class Part
    {
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public int MinimumStock { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsLowStock => StockQuantity <= MinimumStock;
    }
}
