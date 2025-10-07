namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// Сервиз за управление на работни поръчки
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
        /// Взема всички поръчки
        /// </summary>
        public async Task<List<WorkOrder>> GetAllWorkOrdersAsync()
        {
            return await _workOrderRepository.GetAllWithDetailsAsync();
        }

        /// <summary>
        /// Взема поръчка по ID
        /// </summary>
        public async Task<WorkOrder?> GetWorkOrderByIdAsync(int id)
        {
            return await _workOrderRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Взема поръчка по номер
        /// </summary>
        public async Task<WorkOrder?> GetWorkOrderByNumberAsync(string orderNumber)
        {
            return await _workOrderRepository.GetByOrderNumberAsync(orderNumber);
        }

        /// <summary>
        /// Създава нова поръчка
        /// </summary>
        public async Task<WorkOrder> CreateWorkOrderAsync(WorkOrder workOrder)
        {
            // Валидация
            if (workOrder.CustomerId <= 0)
                throw new ArgumentException("Клиентът е задължителен");

            if (workOrder.VehicleId <= 0)
                throw new ArgumentException("Автомобилът е задължителен");

            // Генериране на номер
            workOrder.OrderNumber = await _workOrderRepository.GenerateOrderNumberAsync();
            workOrder.CreatedDate = DateTime.Now;
            workOrder.Status = WorkOrderStatus.Pending;

            return await _workOrderRepository.AddAsync(workOrder);
        }

        /// <summary>
        /// Обновява поръчка
        /// </summary>
        public async Task UpdateWorkOrderAsync(WorkOrder workOrder)
        {
            await _workOrderRepository.UpdateAsync(workOrder);
        }

        /// <summary>
        /// Променя статуса на поръчка
        /// </summary>
        public async Task ChangeStatusAsync(int workOrderId, WorkOrderStatus newStatus)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
            if (workOrder == null)
                throw new ArgumentException("Поръчката не е намерена");

            workOrder.Status = newStatus;

            // Ако е завършена, запиши датата
            if (newStatus == WorkOrderStatus.Completed)
            {
                workOrder.CompletedDate = DateTime.Now;
                
                // Актуализирай последна дата на сервиз за автомобила
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
        /// Взема активните поръчки
        /// </summary>
        public async Task<List<WorkOrder>> GetActiveWorkOrdersAsync()
        {
            return await _workOrderRepository.GetActiveOrdersAsync();
        }

        /// <summary>
        /// Взема поръчки по статус
        /// </summary>
        public async Task<List<WorkOrder>> GetWorkOrdersByStatusAsync(WorkOrderStatus status)
        {
            return await _workOrderRepository.GetByStatusAsync(status);
        }

        /// <summary>
        /// Взема поръчките чакащи части
        /// </summary>
        public async Task<List<WorkOrder>> GetWaitingForPartsAsync()
        {
            return await _workOrderRepository.GetWaitingForPartsAsync();
        }

        /// <summary>
        /// Взема историята на автомобил
        /// </summary>
        public async Task<List<WorkOrder>> GetVehicleHistoryAsync(int vehicleId)
        {
            return await _workOrderRepository.GetByVehicleIdAsync(vehicleId);
        }

        /// <summary>
        /// Изтрива поръчка
        /// </summary>
        public async Task DeleteWorkOrderAsync(int id)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
                throw new ArgumentException("Поръчката не е намерена");

            // Може да се изтрие само ако не е фактурирана
            if (workOrder.Status == WorkOrderStatus.Invoiced)
                throw new InvalidOperationException("Не можете да изтриете фактурирана поръчка");

            await _workOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Статистика за период
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
    /// DTO за статистика на поръчки
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
