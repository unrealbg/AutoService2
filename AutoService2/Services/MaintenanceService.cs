namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// ������ �� ���������� �� ������� �� ���������
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
        /// ����� ������ �������
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetAllSchedulesAsync()
        {
            return await _scheduleRepository.GetAllAsync();
        }

        /// <summary>
        /// ����� ������� �� ��������� ���������
        /// </summary>
        public async Task<List<MaintenanceSchedule>> GetByVehicleIdAsync(int vehicleId)
        {
            var all = await _scheduleRepository.GetAllAsync();
            return all.Where(s => s.VehicleId == vehicleId && s.IsActive).ToList();
        }

        /// <summary>
        /// ����� ������ ����� ������ �� �� �������� �����
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
        /// ����� ���������� ���������
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
        /// ������� ���� ��������� ��� ����� �� ���������
        /// </summary>
        public async Task<List<MaintenanceSchedule>> CheckVehicleMaintenanceAsync(int vehicleId, int currentMileage)
        {
            var schedules = await GetByVehicleIdAsync(vehicleId);
            var today = DateTime.Now;
            
            return schedules.Where(s => s.IsDue(today, currentMileage)).ToList();
        }

        /// <summary>
        /// ������ ��� ������ �� ���������
        /// </summary>
        public async Task<MaintenanceSchedule> CreateScheduleAsync(MaintenanceSchedule schedule)
        {
            // ���������
            if (schedule.VehicleId <= 0)
                throw new ArgumentException("����������� � ������������");

            if (schedule.ServiceTypeId <= 0)
                throw new ArgumentException("����� ������ � ������������");

            return await _scheduleRepository.AddAsync(schedule);
        }

        /// <summary>
        /// ������ ������
        /// </summary>
        public async Task UpdateScheduleAsync(MaintenanceSchedule schedule)
        {
            await _scheduleRepository.UpdateAsync(schedule);
        }

        /// <summary>
        /// ������ ��������� ���������
        /// </summary>
        public async Task RecordMaintenanceAsync(int scheduleId, DateTime performedDate, int performedMileage)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(scheduleId);
            if (schedule == null)
                throw new ArgumentException("�������� �� � �������");

            schedule.LastPerformedDate = performedDate;
            schedule.LastPerformedMileage = performedMileage;

            // ������� ���������� ���������
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
        /// ������ ����������� ������ �� ��� ���������
        /// </summary>
        public async Task CreateDefaultSchedulesForVehicleAsync(int vehicleId)
        {
            var serviceTypes = await _serviceTypeRepository.GetAllAsync();
            var schedulesToCreate = new List<MaintenanceSchedule>();

            // ������ ������� �� ������ � �������������� ��������
            foreach (var serviceType in serviceTypes.Where(s => !string.IsNullOrEmpty(s.RecommendedInterval)))
            {
                var schedule = new MaintenanceSchedule
                {
                    VehicleId = vehicleId,
                    ServiceTypeId = serviceType.Id,
                    IsActive = true
                };

                // ������ �� �� �������� ���������
                if (serviceType.RecommendedInterval.Contains("��"))
                {
                    var kmStr = new string(serviceType.RecommendedInterval.TakeWhile(char.IsDigit).ToArray());
                    if (int.TryParse(kmStr, out int km))
                    {
                        schedule.IntervalKilometers = km;
                    }
                }

                if (serviceType.RecommendedInterval.Contains("�����"))
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
