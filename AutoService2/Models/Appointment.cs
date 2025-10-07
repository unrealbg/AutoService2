namespace AutoService2.Models
{
    // Модел за прием
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
