namespace AutoService2.Services
{
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    /// <summary>
    /// ������ �� ���������� �� �������
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
        /// ����� ������ �������
        /// </summary>
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        /// <summary>
        /// ����� �������
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
        /// ����� ������ �� ID
        /// </summary>
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// ����� ������ � ����� �������
        /// </summary>
        public async Task<Customer?> GetCustomerWithDetailsAsync(int id)
        {
            return await _customerRepository.GetWithFullDetailsAsync(id);
        }

        /// <summary>
        /// ������ ��� ������
        /// </summary>
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            // ���������
            if (string.IsNullOrWhiteSpace(customer.Name))
                throw new ArgumentException("����� � ������������");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ArgumentException("��������� � ������������");

            // �������� �� �������� �������
            var existing = await _customerRepository.GetByPhoneAsync(customer.Phone);
            if (existing != null)
                throw new InvalidOperationException($"������ � ������� {customer.Phone} ���� ����������");

            return await _customerRepository.AddAsync(customer);
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        public async Task DeleteCustomerAsync(int id)
        {
            // �������� �� ������������ ����������
            var vehicles = await _vehicleRepository.GetByCustomerIdAsync(id);
            if (vehicles.Any())
                throw new InvalidOperationException("�� ������ �� �������� ������ � ������������ ����������");

            await _customerRepository.DeleteAsync(id);
        }

        /// <summary>
        /// ����� ������� �� ���
        /// </summary>
        public async Task<List<Customer>> GetCustomersByTypeAsync(CustomerType type)
        {
            return await _customerRepository.GetByTypeAsync(type);
        }

        /// <summary>
        /// ����� ������� �������
        /// </summary>
        public async Task<List<Customer>> GetCompanyCustomersAsync()
        {
            return await GetCustomersByTypeAsync(CustomerType.Company);
        }

        /// <summary>
        /// ����� ������ �������
        /// </summary>
        public async Task<List<Customer>> GetIndividualCustomersAsync()
        {
            return await GetCustomersByTypeAsync(CustomerType.Individual);
        }
    }
}
