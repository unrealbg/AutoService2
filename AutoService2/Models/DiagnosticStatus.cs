namespace AutoService2.Models
{
    /// <summary>
    /// Статус на диагностичния доклад
    /// </summary>
    public enum DiagnosticStatus
    {
        /// <summary>
        /// В процес на изпълнение
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Завършен
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Одобрен от клиента
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Отхвърлен
        /// </summary>
        Rejected = 4
    }
}
