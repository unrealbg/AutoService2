namespace AutoService2.Models
{
    /// <summary>
    /// ��� ������ � �����������
    /// </summary>
    public class ServiceType
    {
        public int Id { get; set; }

        /// <summary>
        /// ��� �� �������� (����. "ENG-001")
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// ��� �� ��������
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// �������� �� ��������
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        public ServiceCategory Category { get; set; }

        /// <summary>
        /// ���� �� ������������
        /// </summary>
        public decimal DefaultPrice { get; set; }

        /// <summary>
        /// �������� ����� �� ���������� (� ������)
        /// </summary>
        public int EstimatedDurationMinutes { get; set; }

        /// <summary>
        /// ������� �� � ��������
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ������� �� �������� �����
        /// </summary>
        public bool RequiresParts { get; set; } = false;

        /// <summary>
        /// �������������� �������� (� �� ��� ������)
        /// </summary>
        public string? RecommendedInterval { get; set; }

        /// <summary>
        /// ���� �� ���������
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
