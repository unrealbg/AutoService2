namespace AutoService2.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using AutoService2.Models;

    /// <summary>
    /// Repository за клиенти
    /// </summary>
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(AutoServiceDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Взема всички клиенти със зареден автомобили
        /// </summary>
        public async Task<List<Customer>> GetAllWithVehiclesAsync()
        {
            return await _dbSet
                .Include(c => c.Vehicles)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Търси клиенти по име
        /// </summary>
        public async Task<List<Customer>> SearchByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return await GetAllAsync();

            return await _dbSet
                .Where(c => c.Name.Contains(name))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Търси клиент по телефон
        /// </summary>
        public async Task<Customer?> GetByPhoneAsync(string phone)
        {
            return await _dbSet
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Phone == phone);
        }

        /// <summary>
        /// Взема клиенти по тип
        /// </summary>
        public async Task<List<Customer>> GetByTypeAsync(CustomerType type)
        {
            return await _dbSet
                .Where(c => c.Type == type)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Взема клиент с всички негови данни (автомобили и поръчки)
        /// </summary>
        public async Task<Customer?> GetWithFullDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Vehicles)
                .Include(c => c.WorkOrders)
                    .ThenInclude(w => w.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
