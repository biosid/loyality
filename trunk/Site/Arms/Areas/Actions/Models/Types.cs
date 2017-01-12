using System;
using System.ComponentModel;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;

namespace Vtb24.Arms.Actions.Models
{
    public enum Types
    {
        // ReSharper disable InconsistentNaming

        [Description("Аддитивное правило")]
        addition,

        [Description("Мультипликативное правило")]
        multiplication,

        [Description("Базовое аддитивное правило")]
        base_addition,

        [Description("Базовое мультипликативное правило")]
        base_multiplication
        
        // ReSharper restore InconsistentNaming
    }

    public static class TypesExtensions
    {
        public static ActionType? Map(this Types? original)
        {
            if (!original.HasValue)
                return null;

            switch (original)
            {
                case Types.addition:
                    return ActionType.Addition;
                case Types.multiplication:
                    return ActionType.Multiplication;
                case Types.base_addition:
                    return ActionType.BaseAddition;
                case Types.base_multiplication:
                    return ActionType.BaseMultiplication;
            }

            return null;
        }

        public static Types Map(this ActionType original)
        {
            switch (original)
            {
                case ActionType.Addition:
                    return Types.addition;
                case ActionType.Multiplication:
                    return Types.multiplication;
                case ActionType.BaseAddition:
                    return Types.base_addition;
                case ActionType.BaseMultiplication:
                    return Types.base_multiplication;
            }

            throw new NotSupportedException(string.Format("Тип механики {0} не поддерживается", original));
        }

        public static ActionType Map(this Types original)
        {
            switch (original)
            {
                case Types.addition:
                    return ActionType.Addition;
                case Types.multiplication:
                    return ActionType.Multiplication;
                case Types.base_addition:
                    return ActionType.BaseAddition;
                case Types.base_multiplication:
                    return ActionType.BaseMultiplication;
            }

            throw new InvalidOperationException("Неверный тип механики");
        }
    }
}
