namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// Сервиз за управление на клиенти
    /// </summary>
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly VehicleRepository _vehicleRepository;

        public CustomerService(
            CustomerRepository customerRepository,
            VehicleRepository vehicleRepository)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Взема всички клиенти
        /// </summary>
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        /// <summary>
        /// Търси клиенти
        /// </summary>
        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCustomersAsync();

            var customers = await _customerRepository.GetAllAsync();
            
            return customers.Where(c =>
                c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (c.Email?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }

        /// <summary>
        /// Взема клиент по ID
        /// </summary>
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Взема клиент с пълни детайли
        /// </summary>
        public async Task<Customer?> GetCustomerWithDetailsAsync(int id)
        {
            return await _customerRepository.GetWithFullDetailsAsync(id);
        }

        /// <summary>
        /// Добавя нов клиент
        /// </summary>
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("Името е задължително");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("Телефонът е задължителен");

            // Проверка за дублиран телефон
            var existing = await _customerRepository.GetByPhoneAsync(customer.Phone);
            if (existing != null)
                throw new InvalidOperationException($"Клиент с телефон {customer.Phone} вече съществува");

            return await _customerRepository.AddAsync(customer);
        }

        /// <summary>
        /// Обновява клиент
        /// </summary>
        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        /// <summary>
        /// Изтрива клиент
        /// </summary>
        public async Task DeleteCustomerAsync(int id)
        {
            // Проверка за съществуващи автомобили
            var vehicles = await _vehicleRepository.GetByCustomerIdAsync(id);
            if (vehicles.Any())
                throw new InvalidOperationException("Не можете да изтриете клиент с регистрирани автомобили");

            await _customerRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Взема клиенти по тип
        /// </summary>
        public async Task<List<Customer>> GetCustomersByTypeAsync(CustomerType type)
        {
            return await _customerRepository.GetByTypeAsync(type);
        }

        /// <summary>
        /// Взема фирмени клиенти
        /// </summary>
        public async Task<List<Customer>> GetCompanyCustomersAsync()
        {
            return await GetCustomersByTypeAsync(CustomerType.Company);
        }

        /// <summary>
        /// Взема частни клиенти
        /// </summary>
        public async Task<List<Customer>> GetIndividualCustomersAsync()
        {
            return await GetCustomersByTypeAsync(CustomerType.Individual);
        }
    }
}
