namespace AutoService2.Models
{
    /// <summary>
    /// ������������ ������ �� ���������
    /// </summary>
    public class DiagnosticReport
    {
        public int Id { get; set; }

        /// <summary>
        /// ����� �� �������
        /// </summary>
        public string ReportNumber { get; set; } = string.Empty;

        /// <summary>
        /// ���������
        /// </summary>
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        /// <summary>
        /// �������� ������� ������� (��� ���)
        /// </summary>
        public int? WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        /// <summary>
        /// ���� �� �������������
        /// </summary>
        public DateTime DiagnosticDate { get; set; } = DateTime.Now;

        /// <summary>
        /// ����� ������
        /// </summary>
        public int CurrentMileage { get; set; }

        /// <summary>
        /// ������� ����� �� �������
        /// </summary>
        public string CustomerComplaint { get; set; } = string.Empty;

        /// <summary>
        /// ������������ ��������
        /// </summary>
        public string FoundIssues { get; set; } = string.Empty;

        /// <summary>
        /// ��������� �� ������
        /// </summary>
        public string Recommendations { get; set; } = string.Empty;

        /// <summary>
        /// �������� �� �������
        /// </summary>
        public WorkOrderPriority Urgency { get; set; } = WorkOrderPriority.Normal;

        /// <summary>
        /// ������ �� �������
        /// </summary>
        public DiagnosticStatus Status { get; set; } = DiagnosticStatus.InProgress;

        /// <summary>
        /// ��������� ���� �� ������
        /// </summary>
        public decimal? EstimatedCost { get; set; }

        /// <summary>
        /// �������/��������
        /// </summary>
        public string? TechnicianName { get; set; }

        /// <summary>
        /// ������ (������ �� ���������, ��������� ��� �������)
        /// </summary>
        public string? PhotoPaths { get; set; }

        /// <summary>
        /// ��� �� ������ (OBD-II ������)
        /// </summary>
        public string? ErrorCodes { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// ���� �� ���������
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// ���� �� ���������/����������
        /// </summary>
        public DateTime? DecisionDate { get; set; }
    }
}
