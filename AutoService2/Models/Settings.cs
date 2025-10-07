namespace AutoService2.Models
{

    // Настройки
    public class Settings
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal VATRate { get; set; } = 0.20m;
        public string Currency { get; set; } = "BGN";
        public decimal HourlyRate { get; set; } = 80.00m;
    }
}
