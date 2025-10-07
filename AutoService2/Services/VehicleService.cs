namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// Сервиз за управление на автомобили
    /// </summary>
    public class VehicleService
    {
        private readonly VehicleRepository _vehicleRepository;
        private readonly WorkOrderRepository _workOrderRepository;

        public VehicleService(
            VehicleRepository vehicleRepository,
            WorkOrderRepository workOrderRepository)
        {
            _vehicleRepository = vehicleRepository;
            _workOrderRepository = workOrderRepository;
        }

        /// <summary>
        /// Взема всички автомобили
        /// </summary>
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllWithCustomerAsync();
        }

        /// <summary>
        /// Търси автомобили
        /// </summary>
        public async Task<List<Vehicle>> SearchVehiclesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllVehiclesAsync();

            var vehicles = await _vehicleRepository.GetAllWithCustomerAsync();
            
            return vehicles.Where(v =>
                v.RegistrationNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.Make.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.VIN.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (v.Customer?.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        /// <summary>
        /// Взема автомобил по ID
        /// </summary>
        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Добавя нов автомобил
        /// </summary>
        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(vehicle.RegistrationNumber))
                throw new ArgumentException("Регистрационният номер е задължителен");

            if (string.IsNullOrWhiteSpace(vehicle.Make))
                throw new ArgumentException("Марката е задължителна");

            // Проверка за дублиран рег. номер
            var existing = await _vehicleRepository.SearchByPlateAsync(vehicle.RegistrationNumber);
            if (existing.Any(v => v.RegistrationNumber.Equals(vehicle.RegistrationNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Автомобил с рег. номер {vehicle.RegistrationNumber} вече съществува");

            vehicle.LastServiceDate = DateTime.Now;
            return await _vehicleRepository.AddAsync(vehicle);
        }

        /// <summary>
        /// Обновява автомобил
        /// </summary>
        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }

        /// <summary>
        /// Изтрива автомобил
        /// </summary>
        public async Task DeleteVehicleAsync(int id)
        {
            // Проверка за съществуващи поръчки
            var orders = await _workOrderRepository.GetByVehicleIdAsync(id);
            if (orders.Any())
                throw new InvalidOperationException("Не можете да изтриете автомобил с налични поръчки");

            await _vehicleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Взема автомобилите на клиент
        /// </summary>
        public async Task<List<Vehicle>> GetCustomerVehiclesAsync(int customerId)
        {
            return await _vehicleRepository.GetByCustomerIdAsync(customerId);
        }

        /// <summary>
        /// Взема автомобили нуждаещи се от преглед
        /// </summary>
        public async Task<List<Vehicle>> GetVehiclesDueForServiceAsync()
        {
            return await _vehicleRepository.GetDueForServiceAsync();
        }

        /// <summary>
        /// Актуализира последна дата на сервиз
        /// </summary>
        public async Task UpdateLastServiceDateAsync(int vehicleId, DateTime date)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle != null)
            {
                vehicle.LastServiceDate = date;
                await _vehicleRepository.UpdateAsync(vehicle);
            }
        }
    }
}
