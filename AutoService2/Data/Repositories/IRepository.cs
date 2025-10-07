namespace AutoService2.Data.Repositories
{
    /// <summary>
    /// Generic repository интерфейс за CRUD операции
    /// </summary>
    /// <typeparam name="T">Entity тип</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Взема всички записи от базата
        /// </summary>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Взема запис по ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Добавя нов запис
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Обновява съществуващ запис
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Изтрива запис по ID
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Проверява дали запис съществува
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Брои записите
        /// </summary>
        Task<int> CountAsync();
    }
}
