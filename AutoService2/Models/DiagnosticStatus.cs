namespace AutoService2.Models
{
    /// <summary>
    /// ������ �� ������������� ������
    /// </summary>
    public enum DiagnosticStatus
    {
        /// <summary>
        /// � ������ �� ����������
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// ��������
        /// </summary>
        Completed = 2,

        /// <summary>
        /// ������� �� �������
        /// </summary>
        Approved = 3,

        /// <summary>
        /// ���������
        /// </summary>
        Rejected = 4
    }
}
