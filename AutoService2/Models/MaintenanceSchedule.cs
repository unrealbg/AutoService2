namespace AutoService2.Models
{
    /// <summary>
    /// ������ �� ������������ � ��������� �� ���������
    /// </summary>
    public class MaintenanceSchedule
    {
        public int Id { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        /// <summary>
        /// ��� ������
        /// </summary>
        public int ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }

        /// <summary>
        /// �������� ����������
        /// </summary>
        public DateTime? LastPerformedDate { get; set; }

        /// <summary>
        /// ������ ��� �������� ����������
        /// </summary>
        public int? LastPerformedMileage { get; set; }

        /// <summary>
        /// �������� ��������� ���������� (����)
        /// </summary>
        public DateTime? NextDueDate { get; set; }

        /// <summary>
        /// �������� ��������� ���������� (������)
        /// </summary>
        public int? NextDueMileage { get; set; }

        /// <summary>
        /// �������� � ������
        /// </summary>
        public int? IntervalMonths { get; set; }

        /// <summary>
        /// �������� � ���������
        /// </summary>
        public int? IntervalKilometers { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// ������� �� � ��������
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ��������� �� � ��������
        /// </summary>
        public bool NotificationSent { get; set; } = false;

        /// <summary>
        /// ���� �� ���������
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// �������� ���� � ����� �� ����������
        /// </summary>
        public bool IsDue(DateTime currentDate, int currentMileage)
        {
            if (!IsActive)
                return false;

            // �������� �� ����
            if (NextDueDate.HasValue && currentDate >= NextDueDate.Value)
                return true;

            // �������� �� ������
            if (NextDueMileage.HasValue && currentMileage >= NextDueMileage.Value)
                return true;

            return false;
        }
    }
}
