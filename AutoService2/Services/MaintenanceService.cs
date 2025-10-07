namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// Сервиз за управление на графици за поддръжка
    /// </summary>
    public class MaintenanceService
    {
        private readonly IRepository<MaintenanceSchedule> _scheduleRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly IRepository<ServiceType> _serviceTypeRepository;

        public MaintenanceService(
            IRepository<MaintenanceSchedule> scheduleRepository,
            VehicleRepository vehicleRepository,
            IRepository<ServiceType> serviceTypeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _vehicleRepository = vehicleRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Вземи всички графици
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetAllSchedulesAsync()
        {
            return await _scheduleRepository.GetAllAsync();
        }

        /// <summary>
        /// Вземи графици за конкретен автомобил
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetByVehicleIdAsync(int vehicleId)
        {
            var all = await _scheduleRepository.GetAllAsync();
            return all.Where(s => s.VehicleId == vehicleId && s.IsActive).ToList();
        }

        /// <summary>
        /// Вземи услуги които трябва да се извършат скоро
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetUpcomingMaintenanceAsync(int daysAhead = 30)
        {
            var all = await _scheduleRepository.GetAllAsync();
            var cutoffDate = DateTime.Now.AddDays(daysAhead);
            
            return all.Where(s => 
                s.IsActive && 
                s.NextDueDate.HasValue && 
                s.NextDueDate.Value <= cutoffDate &&
                s.NextDueDate.Value >= DateTime.Now
            ).OrderBy(s => s.NextDueDate).ToList();
        }

        /// <summary>
        /// Вземи просрочени поддръжки
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetOverdueMaintenanceAsync()
        {
            var all = await _scheduleRepository.GetAllAsync();
            var today = DateTime.Now;
            
            return all.Where(s => 
                s.IsActive && 
                s.NextDueDate.HasValue && 
                s.NextDueDate.Value < today
            ).OrderBy(s => s.NextDueDate).ToList();
        }

        /// <summary>
        /// Провери дали автомобил има нужда от поддръжка
        /// </summary>
        public async Task<List<MaintenanceSchedule>> CheckVehicleMaintenanceAsync(int vehicleId, int currentMileage)
        {
            var schedules = await GetByVehicleIdAsync(vehicleId);
            var today = DateTime.Now;
            
            return schedules.Where(s => s.IsDue(today, currentMileage)).ToList();
        }

        /// <summary>
        /// Създай нов график за поддръжка
        /// </summary>
        public async Task<MaintenanceSchedule> CreateScheduleAsync(MaintenanceSchedule schedule)
        {
            // Валидация
            if (schedule.VehicleId <= 0)
                throw new ArgumentException("Автомобилът е задължителен");

            if (schedule.ServiceTypeId <= 0)
                throw new ArgumentException("Видът услуга е задължителен");

            return await _scheduleRepository.AddAsync(schedule);
        }

        /// <summary>
        /// Обнови график
        /// </summary>
        public async Task UpdateScheduleAsync(MaintenanceSchedule schedule)
        {
            await _scheduleRepository.UpdateAsync(schedule);
        }

        /// <summary>
        /// Запиши извършена поддръжка
        /// </summary>
        public async Task RecordMaintenanceAsync(int scheduleId, DateTime performedDate, int performedMileage)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new ArgumentException("Графикът не е намерен");

            schedule.LastPerformedDate = performedDate;
            schedule.LastPerformedMileage = performedMileage;

            // Изчисли следващото планирано
            if (schedule.IntervalMonths.HasValue)
            {
                schedule.NextDueDate = performedDate.AddMonths(schedule.IntervalMonths.Value);
            }

            if (schedule.IntervalKilometers.HasValue)
            {
                schedule.NextDueMileage = performedMileage + schedule.IntervalKilometers.Value;
            }

            schedule.NotificationSent = false;

            await _scheduleRepository.UpdateAsync(schedule);
        }

        /// <summary>
        /// Създай автоматичен график за нов автомобил
        /// </summary>
        public async Task CreateDefaultSchedulesForVehicleAsync(int vehicleId)
        {
            var serviceTypes = await _serviceTypeRepository.GetAllAsync();
            var schedulesToCreate = new List<MaintenanceSchedule>();

            // Създай графици за услуги с препоръчителен интервал
            foreach (var serviceType in serviceTypes.Where(s => !string.IsNullOrEmpty(s.RecommendedInterval)))
            {
                var schedule = new MaintenanceSchedule
                {
                    VehicleId = vehicleId,
                    ServiceTypeId = serviceType.Id,
                    IsActive = true
                };

                // Опитай се да парсираш интервала
                if (serviceType.RecommendedInterval.Contains("км"))
                {
                    var kmStr = new string(serviceType.RecommendedInterval.TakeWhile(char.IsDigit).ToArray());
                    if (int.TryParse(kmStr, out int km))
                    {
                        schedule.IntervalKilometers = km;
                    }
                }

                if (serviceType.RecommendedInterval.Contains("месец"))
                {
                    var monthStr = new string(serviceType.RecommendedInterval.TakeWhile(char.IsDigit).ToArray());
                    if (int.TryParse(monthStr, out int months))
                    {
                        schedule.IntervalMonths = months;
                        schedule.NextDueDate = DateTime.Now.AddMonths(months);
                    }
                }

                schedulesToCreate.Add(schedule);
            }

            foreach (var schedule in schedulesToCreate)
            {
                await _scheduleRepository.AddAsync(schedule);
            }
        }
    }
}
