namespace AutoService2.Models
{
    // Модел за клиент
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CustomerType Type { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? CompanyId { get; set; }
        public string? Notes { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new();
        public List<WorkOrder> WorkOrders { get; set; } = new();
    }
}
