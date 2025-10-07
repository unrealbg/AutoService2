namespace AutoService2.Models
{
    /// <summary>
    /// Диагностичен доклад за автомобил
    /// </summary>
    public class DiagnosticReport
    {
        public int Id { get; set; }

        /// <summary>
        /// Номер на доклада
        /// </summary>
        public string ReportNumber { get; set; } = string.Empty;

        /// <summary>
        /// Автомобил
        /// </summary>
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        /// <summary>
        /// Свързана работна поръчка (ако има)
        /// </summary>
        public int? WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        /// <summary>
        /// Дата на диагностиката
        /// </summary>
        public DateTime DiagnosticDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Текущ пробег
        /// </summary>
        public int CurrentMileage { get; set; }

        /// <summary>
        /// Основна жалба на клиента
        /// </summary>
        public string CustomerComplaint { get; set; } = string.Empty;

        /// <summary>
        /// Констатирани проблеми
        /// </summary>
        public string FoundIssues { get; set; } = string.Empty;

        /// <summary>
        /// Препоръки за ремонт
        /// </summary>
        public string Recommendations { get; set; } = string.Empty;

        /// <summary>
        /// Спешност на ремонта
        /// </summary>
        public WorkOrderPriority Urgency { get; set; } = WorkOrderPriority.Normal;

        /// <summary>
        /// Статус на доклада
        /// </summary>
        public DiagnosticStatus Status { get; set; } = DiagnosticStatus.InProgress;

        /// <summary>
        /// Прогнозна цена за ремонт
        /// </summary>
        public decimal? EstimatedCost { get; set; }

        /// <summary>
        /// Механик/Диагност
        /// </summary>
        public string? TechnicianName { get; set; }

        /// <summary>
        /// Снимки (пътища до файловете, разделени със запетаи)
        /// </summary>
        public string? PhotoPaths { get; set; }

        /// <summary>
        /// Код за грешка (OBD-II кодове)
        /// </summary>
        public string? ErrorCodes { get; set; }

        /// <summary>
        /// Бележки
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Дата на създаване
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Дата на одобрение/отхвърляне
        /// </summary>
        public DateTime? DecisionDate { get; set; }
    }
}
