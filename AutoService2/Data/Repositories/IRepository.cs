namespace AutoService2.Data.Repositories
{
    /// <summary>
    /// Generic repository ��������� �� CRUD ��������
    /// </summary>
    /// <typeparam name="T">Entity ���</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// ����� ������ ������ �� ������
        /// </summary>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// ����� ����� �� ID
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// ������ ��� �����
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// �������� ����������� �����
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// ������� ����� �� ID
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// ��������� ���� ����� ����������
        /// </summary>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// ���� ��������
        /// </summary>
        Task<int> CountAsync();
    }
}
