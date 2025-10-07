namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Сервиз за управление на видове услуги
    /// </summary>
    public class ServiceTypeService
    {
        private readonly IRepository<ServiceType> _serviceTypeRepository;

        public ServiceTypeService(IRepository<ServiceType> serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        /// <summary>
        /// Вземи всички услуги
        /// </summary>
        public async Task<List<ServiceType>> GetAllServiceTypesAsync()
        {
            return await _serviceTypeRepository.GetAllAsync();
        }

        /// <summary>
        /// Вземи само активни услуги
        /// </summary>
        public async Task<List<ServiceType>> GetActiveServiceTypesAsync()
        {
            var all = await _serviceTypeRepository.GetAllAsync();
            return all.Where(s => s.IsActive).ToList();
        }

        /// <summary>
        /// Вземи услуги по категория
        /// </summary>
        public async Task<List<ServiceType>> GetByCategory(ServiceCategory category)
        {
            var all = await _serviceTypeRepository.GetAllAsync();
            return all.Where(s => s.Category == category && s.IsActive).ToList();
        }

        /// <summary>
        /// Търси услуги по име или код
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
        /// Добави нова услуга
        /// </summary>
        public async Task<ServiceType> AddServiceTypeAsync(ServiceType serviceType)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(serviceType.Code))
                throw new ArgumentException("Кодът на услугата е задължителен");

            if (string.IsNullOrWhiteSpace(serviceType.Name))
                throw new ArgumentException("Името на услугата е задължително");

            // Проверка за дублиращ се код
            var existing = await _serviceTypeRepository.GetAllAsync();
            if (existing.Any(s => s.Code == serviceType.Code))
                throw new ArgumentException($"Услуга с код {serviceType.Code} вече съществува");

            return await _serviceTypeRepository.AddAsync(serviceType);
        }

        /// <summary>
        /// Обнови услуга
        /// </summary>
        public async Task UpdateServiceTypeAsync(ServiceType serviceType)
        {
            await _serviceTypeRepository.UpdateAsync(serviceType);
        }

        /// <summary>
        /// Изтрий услуга (деактивирай)
        /// </summary>
        public async Task DeactivateServiceTypeAsync(int id)
        {
            var serviceType = await _serviceTypeRepository.GetByIdAsync(id);
            if (serviceType == null)
                throw new ArgumentException("Услугата не е намерена");

            serviceType.IsActive = false;
            await _serviceTypeRepository.UpdateAsync(serviceType);
        }

        /// <summary>
        /// Вземи популярни услуги (по брой поръчки)
        /// </summary>
        public async Task<List<ServiceType>> GetPopularServicesAsync(int count = 10)
        {
            // TODO: Имплементирай логика за преброяване на поръчките
            var all = await GetActiveServiceTypesAsync();
            return all.Take(count).ToList();
        }

        /// <summary>
        /// Изчисли времето за изпълнение на набор от услуги
        /// </summary>
        public int CalculateTotalDuration(List<int> serviceTypeIds, List<ServiceType> services)
        {
            return services
                .Where(s => serviceTypeIds.Contains(s.Id))
                .Sum(s => s.EstimatedDurationMinutes);
        }

        /// <summary>
        /// Изчисли общата цена на набор от услуги
        /// </summary>
        public decimal CalculateTotalPrice(List<int> serviceTypeIds, List<ServiceType> services)
        {
            return services
                .Where(s => serviceTypeIds.Contains(s.Id))
                .Sum(s => s.DefaultPrice);
        }
    }
}
