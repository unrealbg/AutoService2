namespace AutoService2.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using AutoService2.Models;

    /// <summary>
    /// Repository за автомобили с допълнителни методи
    /// </summary>
    public class VehicleRepository : Repository<Vehicle>
    {
        public VehicleRepository(AutoServiceDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Взема всички автомобили със зареден клиент
        /// </summary>
        public async Task<List<Vehicle>> GetAllWithCustomerAsync()
        {
            return await _dbSet
                .Include(v => v.Customer)
                .OrderByDescending(v => v.LastServiceDate)
                .ToListAsync();
        }

        /// <summary>
        /// Търси по регистрационен номер
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
        /// Търси по VIN номер
        /// </summary>
        public async Task<Vehicle?> GetByVinAsync(string vin)
        {
            return await _dbSet
                .Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.VIN == vin);
        }

        /// <summary>
        /// Взема автомобилите на конкретен клиент
        /// </summary>
        public async Task<List<Vehicle>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet
                .Where(v => v.CustomerId == customerId)
                .OrderByDescending(v => v.LastServiceDate)
                .ToListAsync();
        }

        /// <summary>
        /// Взема автомобили нуждаещи се от сервиз (над 6 месеца)
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
