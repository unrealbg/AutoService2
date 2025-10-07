namespace AutoService2.Models
{
    public enum InvoiceStatus
    {
        Draft,      // Чернова
        Issued,     // Издадена
        Paid,       // Платена
        Overdue,    // Просрочена
        Cancelled   // Анулирана
    }
}
