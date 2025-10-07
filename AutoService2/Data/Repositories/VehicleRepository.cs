namespace AutoService2.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using AutoService2.Models;

    /// <summary>
    /// Repository �� ���������� � ������������ ������
    /// </summary>
    public class VehicleRepository : Repository<Vehicle>
    {
        public VehicleRepository(AutoServiceDbContext context) : base(context)
        {
        }

        /// <summary>
        /// ����� ������ ���������� ��� ������� ������
        /// </summary>
        public async Task<List<Vehicle>> GetAllWithCustomerAsync()
        {
            return await _dbSet
                .Include(v => v.Customer)
                .OrderByDescending(v => v.LastServiceDate)
                .ToListAsync();
        }

        /// <summary>
        /// ����� �� �������������� �����
        /// </summary>
        public async Task<List<Vehicle>> SearchByPlateAsync(string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
                return await GetAllWithCustomerAsync();

            return await _dbSet
                .Include(v => v.Customer)
                .Where(v => v.RegistrationNumber.Contains(plate))
                .ToListAsync();
        }

        /// <summary>
        /// ����� �� VIN �����
        /// </summary>
        public async Task<Vehicle?> GetByVinAsync(string vin)
        {
            return await _dbSet
                .Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.VIN == vin);
        }

        /// <summary>
        /// ����� ������������ �� ��������� ������
        /// </summary>
        public async Task<List<Vehicle>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(v => v.CustomerId == customerId)
                .OrderByDescending(v => v.LastServiceDate)
                .ToListAsync();
        }

        /// <summary>
        /// ����� ���������� �������� �� �� ������ (��� 6 ������)
        /// </summary>
        public async Task<List<Vehicle>> GetDueForServiceAsync()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            return await _dbSet
                .Include(v => v.Customer)
                .Where(v => v.LastServiceDate < sixMonthsAgo)
                .OrderBy(v => v.LastServiceDate)
                .ToListAsync();
        }
    }
}
