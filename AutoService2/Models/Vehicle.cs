namespace AutoService2.Models
{
    // Модел за автомобил
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public int Year { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime LastServiceDate { get; set; }
        public List<WorkOrder> WorkOrders { get; set; } = new();
    }
}
