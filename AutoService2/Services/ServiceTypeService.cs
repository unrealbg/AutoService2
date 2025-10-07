namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// ������ �� ���������� �� ������ ������
    /// </summary>
    public class ServiceTypeService
    {
        private readonly IRepository<ServiceType> _serviceTypeRepository;

        public ServiceTypeService(IRepository<ServiceType> serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// ����� ������ ������
        /// </summary>
        public async Task<List<ServiceType>> GetAllServiceTypesAsync()
        {
            return await _serviceTypeRepository.GetAllAsync();
        }

        /// <summary>
        /// ����� ���� ������� ������
        /// </summary>
        public async Task<List<ServiceType>> GetActiveServiceTypesAsync()
        {
            var all = await _serviceTypeRepository.GetAllAsync();
            return all.Where(s => s.IsActive).ToList();
        }

        /// <summary>
        /// ����� ������ �� ���������
        /// </summary>
        public async Task<List<ServiceType>> GetByCategory(ServiceCategory category)
        {
            var all = await _serviceTypeRepository.GetAllAsync();
            return all.Where(s => s.Category == category && s.IsActive).ToList();
        }

        /// <summary>
        /// ����� ������ �� ��� ��� ���
        /// </summary>
        public async Task<List<ServiceType>> SearchServiceTypesAsync(string searchTerm)
        {
            var all = await _serviceTypeRepository.GetAllAsync();
            return all.Where(s =>
                s.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                s.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        /// <summary>
        /// ������ ���� ������
        /// </summary>
        public async Task<ServiceType> AddServiceTypeAsync(ServiceType serviceType)
        {
            // ���������
            if (string.IsNullOrWhiteSpace(serviceType.Code))
                throw new ArgumentException("����� �� �������� � ������������");

            if (string.IsNullOrWhiteSpace(serviceType.Name))
                throw new ArgumentException("����� �� �������� � ������������");

            // �������� �� �������� �� ���
            var existing = await _serviceTypeRepository.GetAllAsync();
            if (existing.Any(s => s.Code == serviceType.Code))
                throw new ArgumentException($"������ � ��� {serviceType.Code} ���� ����������");

            return await _serviceTypeRepository.AddAsync(serviceType);
        }

        /// <summary>
        /// ������ ������
        /// </summary>
        public async Task UpdateServiceTypeAsync(ServiceType serviceType)
        {
            await _serviceTypeRepository.UpdateAsync(serviceType);
        }

        /// <summary>
        /// ������ ������ (�����������)
        /// </summary>
        public async Task DeactivateServiceTypeAsync(int id)
        {
            var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
            if (serviceType == null)
                throw new ArgumentException("�������� �� � ��������");

            serviceType.IsActive = false;
            await _serviceTypeRepository.UpdateAsync(serviceType);
        }

        /// <summary>
        /// ����� ��������� ������ (�� ���� �������)
        /// </summary>
        public async Task<List<ServiceType>> GetPopularServicesAsync(int count = 10)
        {
            // TODO: ������������� ������ �� ����������� �� ���������
            var all = await GetActiveServiceTypesAsync();
            return all.Take(count).ToList();
        }

        /// <summary>
        /// ������� ������� �� ���������� �� ����� �� ������
        /// </summary>
        public int CalculateTotalDuration(List<int> serviceTypeIds, List<ServiceType> services)
        {
            return services
                .Where(s => serviceTypeIds.Contains(s.Id))
                .Sum(s => s.EstimatedDurationMinutes);
        }

        /// <summary>
        /// ������� ������ ���� �� ����� �� ������
        /// </summary>
        public decimal CalculateTotalPrice(List<int> serviceTypeIds, List<ServiceType> services)
        {
            return services
                .Where(s => serviceTypeIds.Contains(s.Id))
                .Sum(s => s.DefaultPrice);
        }
    }
}
