namespace AutoService2.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using AutoService2.Models;

    /// <summary>
    /// Repository за работни поръчки
    /// </summary>
    public class WorkOrderRepository : Repository<WorkOrder>
    {
        public WorkOrderRepository(AutoServiceDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Взема всички поръчки със зареден данни
        /// </summary>
        public async Task<List<WorkOrder>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Include(w => w.Items)
                    .ThenInclude(i => i.Part)
                .OrderByDescending(w => w.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Взема поръчка по номер
        /// </summary>
        public async Task<WorkOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Include(w => w.Items)
                    .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(w => w.OrderNumber == orderNumber);
        }

        /// <summary>
        /// Взема поръчки по статус
        /// </summary>
        public async Task<List<WorkOrder>> GetByStatusAsync(WorkOrderStatus status)
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Where(w => w.Status == status)
                .OrderByDescending(w => w.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Взема активните поръчки
        /// </summary>
        public async Task<List<WorkOrder>> GetActiveOrdersAsync()
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Where(w => w.Status != WorkOrderStatus.Completed &&
                           w.Status != WorkOrderStatus.Invoiced &&
                           w.Status != WorkOrderStatus.Cancelled)
                .OrderByDescending(w => w.Priority)
                .ThenBy(w => w.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Взема поръчките чакащи части
        /// </summary>
        public async Task<List<WorkOrder>> GetWaitingForPartsAsync()
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Vehicle)
                .Where(w => w.Status == WorkOrderStatus.WaitingForParts)
                .ToListAsync();
        }

        /// <summary>
        /// Взема поръчките на автомобил
        /// </summary>
        public async Task<List<WorkOrder>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet
                .Include(w => w.Customer)
                .Include(w => w.Items)
                .Where(w => w.VehicleId == vehicleId)
                .OrderByDescending(w => w.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Генерира следващ номер на поръчка
        /// </summary>
        public async Task<string> GenerateOrderNumberAsync()
        {
            var year = DateTime.Now.Year;
            var lastOrder = await _dbSet
                .Where(w => w.OrderNumber.StartsWith($"WO-{year}"))
                .OrderByDescending(w => w.OrderNumber)
                .FirstOrDefaultAsync();

            if (lastOrder == null)
                return $"WO-{year}-0001";

            var lastNumber = int.Parse(lastOrder.OrderNumber.Split('-').Last());
            return $"WO-{year}-{(lastNumber + 1):D4}";
        }

        /// <summary>
        /// Статистика за период
        /// </summary>
        public async Task<(int Total, decimal Revenue)> GetStatisticsAsync(DateTime from, DateTime to)
        {
            var orders = await _dbSet
                .Where(w => w.CreatedDate >= from && w.CreatedDate <= to &&
                           (w.Status == WorkOrderStatus.Completed || w.Status == WorkOrderStatus.Invoiced))
                .ToListAsync();

            return (orders.Count, orders.Sum(w => w.TotalCost));
        }
    }
}
