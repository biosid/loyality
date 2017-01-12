namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    /// <summary>
    /// Статус импорта
    /// </summary>
    public enum ImportTaskStatuses
    {
        /// <summary>
        /// Ожидание - задача на имопртирование каталога только создана и еще не обрабатывалась.
        /// </summary>
        Waiting = 0,

        /// <summary>
        /// Загрузка xml-файла в каталог системы.
        /// </summary>
        Loading = 2,

        /// <summary>
        /// Загрузка выполнена
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Загрузка отменена пользователем
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// Критическая ошибка не позволяющая загрузить хотя бы один товар, например, xml-файл не найден.
        /// </summary>
        Error = 5,
    }

    public static class LoadTaskStatusesHelper
    {
        /// <summary>
        /// Возвращает <c>true</c> если статус финальный.
        /// </summary>
        /// <param name="status">
        /// Анализируемый статус.
        /// </param>
        /// <returns>
        /// <c>True</c> если статус финальный.
        /// </returns>
        public static bool IsFinalStatus(this ImportTaskStatuses status)
        {
            switch (status)
            {
                 case ImportTaskStatuses.Canceled:
                 case ImportTaskStatuses.Completed:
                 case ImportTaskStatuses.Error:
                    return true;
                default:
                    return false;
            }
        }
    }
}
