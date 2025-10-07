namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// ������ �� ���������� �� ����������
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
        /// ����� ������ ����������
        /// </summary>
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllWithCustomerAsync();
        }

        /// <summary>
        /// ����� ����������
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
        /// ����� ��������� �� ID
        /// </summary>
        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// ������ ��� ���������
        /// </summary>
        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            // ���������
            if (string.IsNullOrWhiteSpace(vehicle.RegistrationNumber))
                throw new ArgumentException("���������������� ����� � ������������");

            if (string.IsNullOrWhiteSpace(vehicle.Make))
                throw new ArgumentException("������� � ������������");

            // �������� �� �������� ���. �����
            var existing = await _vehicleRepository.SearchByPlateAsync(vehicle.RegistrationNumber);
            if (existing.Any(v => v.RegistrationNumber.Equals(vehicle.RegistrationNumber, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"��������� � ���. ����� {vehicle.RegistrationNumber} ���� ����������");

            vehicle.LastServiceDate = DateTime.Now;
            return await _vehicleRepository.AddAsync(vehicle);
        }

        /// <summary>
        /// �������� ���������
        /// </summary>
        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            await _vehicleRepository.UpdateAsync(vehicle);
        }

        /// <summary>
        /// ������� ���������
        /// </summary>
        public async Task DeleteVehicleAsync(int id)
        {
            // �������� �� ������������ �������
            var orders = await _workOrderRepository.GetByVehicleIdAsync(id);
            if (orders.Any())
                throw new InvalidOperationException("�� ������ �� �������� ��������� � ������� �������");

            await _vehicleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// ����� ������������ �� ������
        /// </summary>
        public async Task<List<Vehicle>> GetCustomerVehiclesAsync(int customerId)
        {
            return await _vehicleRepository.GetByCustomerIdAsync(customerId);
        }

        /// <summary>
        /// ����� ���������� �������� �� �� �������
        /// </summary>
        public async Task<List<Vehicle>> GetVehiclesDueForServiceAsync()
        {
            return await _vehicleRepository.GetDueForServiceAsync();
        }

        /// <summary>
        /// ����������� �������� ���� �� ������
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
