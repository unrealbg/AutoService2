namespace AutoService2.Models
{
    public enum WorkOrderStatus
    {
        Pending,        // ������
        InProgress,     // � ������
        WaitingForParts,// ���� �����
        Completed,      // ���������
        Invoiced,       // �����������
        Cancelled       // ��������
    }
}
