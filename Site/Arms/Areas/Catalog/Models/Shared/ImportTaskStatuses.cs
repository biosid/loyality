using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models;

namespace Vtb24.Arms.Catalog.Models.Shared
{
    public enum ImportTaskStatuses
    {
        // ReSharper disable InconsistentNaming
        [Description("Неподдерживаемый статус")]
        unknown = 0,

        [Description("В очереди")]
        waiting,

        [Description("Идёт загрузка")]
        loading,

        [Description("Загрузка остановлена")]
        canceled,

        [Description("Ошибка загрузки")]
        error,

        [Description("Загружен")]
        completed

        // ReSharper restore InconsistentNaming
    }

    public static class ImportTaskStatusesExtensions
    {
        public static ImportTaskStatuses Map(this ImportTaskStatus original)
        {
            switch (original)
            {
                case ImportTaskStatus.Waiting:
                    return ImportTaskStatuses.waiting;
                case ImportTaskStatus.Loading:
                    return ImportTaskStatuses.loading;
                case ImportTaskStatus.Canceled:
                    return ImportTaskStatuses.canceled;
                case ImportTaskStatus.Error:
                    return ImportTaskStatuses.error;
                case ImportTaskStatus.Completed:
                    return ImportTaskStatuses.completed;
            }

            return ImportTaskStatuses.unknown;
        }
    }
}
