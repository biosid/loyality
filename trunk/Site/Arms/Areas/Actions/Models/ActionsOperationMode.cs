using System;

namespace Vtb24.Arms.Actions.Models
{
    public enum ActionsOperationMode
    {
        Actual,
        Archive,
        Search
    }

    public static class ActionsOperationModeExtension
    {
        public static string GetTitle(this ActionsOperationMode mode)
        {
            switch (mode)
            {
                case ActionsOperationMode.Actual:
                    return "Текущие механики";
                case ActionsOperationMode.Archive:
                    return "Завершённые механики";
                case ActionsOperationMode.Search:
                    return "Поиск механик";
            }

            throw new NotSupportedException(string.Format("Режим управления ценообразованием {0} не поддерживается", mode));
        }
    }
}
