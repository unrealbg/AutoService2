namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// ������ �� ���������� �� ������� �������
    /// </summary>
    public class WorkOrderService
    {
        private readonly WorkOrderRepository _workOrderRepository;
        private readonly VehicleRepository _vehicleRepository;

        public WorkOrderService(
            WorkOrderRepository workOrderRepository,
            VehicleRepository vehicleRepository)
        {
            _workOrderRepository = workOrderRepository;
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// ����� ������ �������
        /// </summary>
        public async Task<List<WorkOrder>> GetAllWorkOrdersAsync()
        {
            return await _workOrderRepository.GetAllWithDetailsAsync();
        }

        /// <summary>
        /// ����� ������� �� ID
        /// </summary>
        public async Task<WorkOrder?> GetWorkOrderByIdAsync(int id)
        {
            return await _workOrderRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// ����� ������� �� �����
        /// </summary>
        public async Task<WorkOrder?> GetWorkOrderByNumberAsync(string orderNumber)
        {
            return await _workOrderRepository.GetByOrderNumberAsync(orderNumber);
        }

        /// <summary>
        /// ������� ���� �������
        /// </summary>
        public async Task<WorkOrder> CreateWorkOrderAsync(WorkOrder workOrder)
        {
            // ���������
            if (workOrder.CustomerId <= 0)
                throw new ArgumentException("�������� � ������������");

            if (workOrder.VehicleId <= 0)
                throw new ArgumentException("����������� � ������������");

            // ���������� �� �����
            workOrder.OrderNumber = await _workOrderRepository.GenerateOrderNumberAsync();
            workOrder.CreatedDate = DateTime.Now;
            workOrder.Status = WorkOrderStatus.Pending;

            return await _workOrderRepository.AddAsync(workOrder);
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        public async Task UpdateWorkOrderAsync(WorkOrder workOrder)
        {
            await _workOrderRepository.UpdateAsync(workOrder);
        }

        /// <summary>
        /// ������� ������� �� �������
        /// </summary>
        public async Task ChangeStatusAsync(int workOrderId, WorkOrderStatus newStatus)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
            if (workOrder == null)
                throw new ArgumentException("��������� �� � ��������");

            workOrder.Status = newStatus;

            // ��� � ���������, ������ ������
            if (newStatus == WorkOrderStatus.Completed)
            {
                workOrder.CompletedDate = DateTime.Now;
                
                // ������������ �������� ���� �� ������ �� ����������
                var vehicle = await _vehicleRepository.GetByIdAsync(workOrder.VehicleId);
                if (vehicle != null)
                {
                    vehicle.LastServiceDate = DateTime.Now;
                    await _vehicleRepository.UpdateAsync(vehicle);
                }
            }

            await _workOrderRepository.UpdateAsync(workOrder);
        }

        /// <summary>
        /// ����� ��������� �������
        /// </summary>
        public async Task<List<WorkOrder>> GetActiveWorkOrdersAsync()
        {
            return await _workOrderRepository.GetActiveOrdersAsync();
        }

        /// <summary>
        /// ����� ������� �� ������
        /// </summary>
        public async Task<List<WorkOrder>> GetWorkOrdersByStatusAsync(WorkOrderStatus status)
        {
            return await _workOrderRepository.GetByStatusAsync(status);
        }

        /// <summary>
        /// ����� ��������� ������ �����
        /// </summary>
        public async Task<List<WorkOrder>> GetWaitingForPartsAsync()
        {
            return await _workOrderRepository.GetWaitingForPartsAsync();
        }

        /// <summary>
        /// ����� ��������� �� ���������
        /// </summary>
        public async Task<List<WorkOrder>> GetVehicleHistoryAsync(int vehicleId)
        {
            return await _workOrderRepository.GetByVehicleIdAsync(vehicleId);
        }

        /// <summary>
        /// ������� �������
        /// </summary>
        public async Task DeleteWorkOrderAsync(int id)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
                throw new ArgumentException("��������� �� � ��������");

            // ���� �� �� ������ ���� ��� �� � �����������
            if (workOrder.Status == WorkOrderStatus.Invoiced)
                throw new InvalidOperationException("�� ������ �� �������� ����������� �������");

            await _workOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// ���������� �� ������
        /// </summary>
        public async Task<WorkOrderStatistics> GetStatisticsAsync(DateTime from, DateTime to)
        {
            var (total, revenue) = await _workOrderRepository.GetStatisticsAsync(from, to);
            var activeOrders = await GetActiveWorkOrdersAsync();
            var waitingForParts = await GetWaitingForPartsAsync();

            return new WorkOrderStatistics
            {
                TotalOrders = total,
                TotalRevenue = revenue,
                ActiveOrders = activeOrders.Count,
                WaitingForParts = waitingForParts.Count,
                AverageOrderValue = total > 0 ? revenue / total : 0
            };
        }
    }

    /// <summary>
    /// DTO �� ���������� �� �������
    /// </summary>
    public class WorkOrderStatistics
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveOrders { get; set; }
        public int WaitingForParts { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
