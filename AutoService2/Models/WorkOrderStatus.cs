namespace AutoService2.Models
{
    public enum WorkOrderStatus
    {
        Pending,        // Чакаща
        InProgress,     // В процес
        WaitingForParts,// Чака части
        Completed,      // Завършена
        Invoiced,       // Фактурирана
        Cancelled       // Отказана
    }
}
