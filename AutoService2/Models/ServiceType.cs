namespace AutoService2.Models
{
    /// <summary>
    /// Вид услуга в автосервиза
    /// </summary>
    public class ServiceType
    {
        public int Id { get; set; }

        /// <summary>
        /// Код на услугата (напр. "ENG-001")
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Име на услугата
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание на услугата
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public ServiceCategory Category { get; set; }

        /// <summary>
        /// Цена по подразбиране
        /// </summary>
        public decimal DefaultPrice { get; set; }

        /// <summary>
        /// Очаквано време за изпълнение (в минути)
        /// </summary>
        public int EstimatedDurationMinutes { get; set; }

        /// <summary>
        /// Активна ли е услугата
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Изисква ли резервни части
        /// </summary>
        public bool RequiresParts { get; set; } = false;

        /// <summary>
        /// Препоръчителен интервал (в км или месеци)
        /// </summary>
        public string? RecommendedInterval { get; set; }

        /// <summary>
        /// Дата на създаване
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
