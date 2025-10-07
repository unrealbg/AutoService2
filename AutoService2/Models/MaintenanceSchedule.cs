namespace AutoService2.Models
{
    /// <summary>
    /// График за профилактика и поддръжка на автомобил
    /// </summary>
    public class MaintenanceSchedule
    {
        public int Id { get; set; }

        /// <summary>
        /// Автомобил
        /// </summary>
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        /// <summary>
        /// Вид услуга
        /// </summary>
        public int ServiceTypeId { get; set; }
        public ServiceType? ServiceType { get; set; }

        /// <summary>
        /// Последно извършване
        /// </summary>
        public DateTime? LastPerformedDate { get; set; }

        /// <summary>
        /// Пробег при последно извършване
        /// </summary>
        public int? LastPerformedMileage { get; set; }

        /// <summary>
        /// Следващо планирано изпълнение (дата)
        /// </summary>
        public DateTime? NextDueDate { get; set; }

        /// <summary>
        /// Следващо планирано изпълнение (пробег)
        /// </summary>
        public int? NextDueMileage { get; set; }

        /// <summary>
        /// Интервал в месеци
        /// </summary>
        public int? IntervalMonths { get; set; }

        /// <summary>
        /// Интервал в километри
        /// </summary>
        public int? IntervalKilometers { get; set; }

        /// <summary>
        /// Бележки
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Активен ли е графикът
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Изпратено ли е известие
        /// </summary>
        public bool NotificationSent { get; set; } = false;

        /// <summary>
        /// Дата на създаване
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Проверка дали е време за обслужване
        /// </summary>
        public bool IsDue(DateTime currentDate, int currentMileage)
        {
            if (!IsActive)
                return false;

            // Проверка по дата
            if (NextDueDate.HasValue && currentDate >= NextDueDate.Value)
                return true;

            // Проверка по пробег
            if (NextDueMileage.HasValue && currentMileage >= NextDueMileage.Value)
                return true;

            return false;
        }
    }
}
