namespace AutoService2.Services
{
    using AutoService2.Models;
    using AutoService2.Data.Repositories;

    // Сервиз за данни на таблото
    public class DashboardService
    {
        private readonly WorkOrderRepository _workOrderRepository;
        private readonly Repository<Appointment> _appointmentRepository;
        private readonly Repository<Invoice> _invoiceRepository;

        public DashboardService(
            WorkOrderRepository workOrderRepository,
            Repository<Appointment> appointmentRepository,
            Repository<Invoice> invoiceRepository)
        {
            _workOrderRepository = workOrderRepository;
            _appointmentRepository = appointmentRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var activeOrders = await _workOrderRepository.GetActiveOrdersAsync();
            var waitingForParts = await _workOrderRepository.GetWaitingForPartsAsync();
            
            var today = DateTime.Today;
            var allAppointments = await _appointmentRepository.GetAllAsync();
            var todayAppointments = allAppointments
                .Count(a => a.ScheduledDateTime.Date == today);

            var allInvoices = await _invoiceRepository.GetAllAsync();
            var unpaidInvoices = allInvoices.Where(i => i.Status != InvoiceStatus.Paid).ToList();
            var overdueInvoices = unpaidInvoices
                .Count(i => i.DueDate < DateTime.Now && i.Status == InvoiceStatus.Issued);

            var stats = new DashboardStats
            {
                ActiveWorkOrders = activeOrders.Count,
                HighPriorityCount = activeOrders.Count(w => w.Priority == WorkOrderPriority.High || w.Priority == WorkOrderPriority.Emergency),
                TodayAppointments = todayAppointments,
                WaitingForParts = waitingForParts.Count,
                UnpaidAmount = unpaidInvoices.Sum(i => i.Total),
                UnpaidInvoices = unpaidInvoices.Count,
                OverdueInvoices = overdueInvoices
            };

            return stats;
        }

        public async Task<List<WorkOrderSummary>> GetRecentWorkOrdersAsync(int count = 10)
        {
            var orders = await _workOrderRepository.GetAllWithDetailsAsync();
            
            return orders
                .Take(count)
                .Select(w => new WorkOrderSummary
                {
                    OrderNumber = w.OrderNumber,
                    CustomerName = w.Customer?.Name ?? "N/A",
                    VehicleInfo = $"{w.Vehicle?.Make} {w.Vehicle?.Model} ({w.Vehicle?.RegistrationNumber})",
                    Status = GetStatusText(w.Status),
                    StatusClass = GetStatusClass(w.Status),
                    Amount = w.Status == WorkOrderStatus.Completed || w.Status == WorkOrderStatus.Invoiced ? w.TotalCost : null
                })
                .ToList();
        }

        public async Task<List<AppointmentSummary>> GetUpcomingAppointmentsAsync(int days = 7)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var upcoming = appointments
                .Where(a => a.ScheduledDateTime >= DateTime.Now && 
                           a.ScheduledDateTime <= DateTime.Now.AddDays(days) &&
                           a.Status != AppointmentStatus.Cancelled)
                .OrderBy(a => a.ScheduledDateTime)
                .Take(10)
                .ToList();

            // Ако няма данни в БД, върни примерни
            if (!upcoming.Any())
            {
                return new List<AppointmentSummary>
                {
                    new() { DateTime = "07.10 14:30", CustomerName = "Н. Стоянов", VehicleName = "Opel Astra", ServiceType = "Смяна масло" },
                    new() { DateTime = "07.10 16:00", CustomerName = "Ramos EOOD", VehicleName = "Renault Master", ServiceType = "Спирачна система" },
                    new() { DateTime = "08.10 09:00", CustomerName = "П. Димова", VehicleName = "Hyundai i30", ServiceType = "Диагностика" }
                };
            }

            return upcoming.Select(a => new AppointmentSummary
            {
                DateTime = a.ScheduledDateTime.ToString("dd.MM HH:mm"),
                CustomerName = a.Customer?.Name ?? "N/A",
                VehicleName = a.Vehicle?.Make + " " + a.Vehicle?.Model ?? "N/A",
                ServiceType = a.ServiceType
            }).ToList();
        }

        private string GetStatusText(WorkOrderStatus status)
        {
            return status switch
            {
                WorkOrderStatus.Pending => "Чакаща",
                WorkOrderStatus.InProgress => "В процес",
                WorkOrderStatus.WaitingForParts => "Части",
                WorkOrderStatus.Completed => "Готова",
                WorkOrderStatus.Invoiced => "Фактурирана",
                WorkOrderStatus.Cancelled => "Отказана",
                _ => status.ToString()
            };
        }

        private string GetStatusClass(WorkOrderStatus status)
        {
            return status switch
            {
                WorkOrderStatus.Completed => "ok",
                WorkOrderStatus.Invoiced => "ok",
                WorkOrderStatus.WaitingForParts => "warn",
                WorkOrderStatus.Cancelled => "bad",
                _ => ""
            };
        }
    }

    // DTOs за таблото
    public class DashboardStats
    {
        public int ActiveWorkOrders { get; set; }
        public int HighPriorityCount { get; set; }
        public int TodayAppointments { get; set; }
        public int WaitingForParts { get; set; }
        public decimal UnpaidAmount { get; set; }
        public int UnpaidInvoices { get; set; }
        public int OverdueInvoices { get; set; }
    }

    public class WorkOrderSummary
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusClass { get; set; } = string.Empty;
        public decimal? Amount { get; set; }
        public string AmountDisplay => Amount.HasValue ? $"{Amount:F2} лв" : "—";
    }

    public class AppointmentSummary
    {
        public string DateTime { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehicleName { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
    }
}
