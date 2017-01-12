namespace RapidSoft.Loaylty.PromoAction.Api.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Extensions;

    /// <summary>
    /// Класс содержит вспомогательные методы для работы с <see cref="RuleTypes"/>.
    /// </summary>
    public static class RuleTypeExtensions
    {
        public static bool IsBase(this RuleTypes ruleType)
        {
            return ruleType == RuleTypes.BaseAddition || ruleType == RuleTypes.BaseMultiplication;
        }

        /// <summary>
        /// Возвращает коэффициент по умолчания для типа правил.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <returns>
        /// Коэффициент по умолчания.
        /// </returns>
        public static decimal GetDefaultFactor(this RuleTypes ruleType)
        {
            switch (ruleType)
            {
                case RuleTypes.BaseMultiplication:
                case RuleTypes.Multiplication:
                    {
                        return 1m;
                    }

                case RuleTypes.BaseAddition:
                case RuleTypes.Addition:
                    {
                        return 0m;
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип правила {0} не поддерживается", ruleType));
                    }
            }
        }

        /// <summary>
        /// Возвращает оператор вычисления для типа правил: '+' или '*'.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <returns>
        /// Оператор вычисления: '+' или '*'.
        /// </returns>
        public static char GetSqlOperation(this RuleTypes ruleType)
        {
            switch (ruleType)
            {
                case RuleTypes.BaseMultiplication:
                case RuleTypes.Multiplication:
                    {
                        return '*';
                    }

                case RuleTypes.BaseAddition:
                case RuleTypes.Addition:
                    {
                        return '+';
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип правила {0} не поддерживается", ruleType));
                    }
            }
        }

        /// <summary>
        /// Выполняет вычисление суммы или произведения в зависимости от типа правила.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <param name="args">
        /// Набор коэффициентов.
        /// </param>
        /// <returns>
        /// Вычисленный результат.
        /// </returns>
        public static decimal Aggregate(this RuleTypes ruleType, params decimal[] args)
        {
            switch (ruleType)
            {
                case RuleTypes.Addition:
                case RuleTypes.BaseAddition:
                    {
                        return args.Sum();
                    }

                case RuleTypes.Multiplication:
                case RuleTypes.BaseMultiplication:
                    {
                        return args.Product();
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип правила {0} не поддерживается", ruleType));
                    }
            }
        }

        /// <summary>
        /// Выполняет вычисление суммы или произведения в зависимости от типа правила.
        /// </summary>
        /// <param name="ruleType">
        /// Тип правила.
        /// </param>
        /// <param name="args">
        /// Коллекция коэффициентов.
        /// </param>
        /// <returns>
        /// Вычисленный результат.
        /// </returns>
        public static decimal Aggregate(this RuleTypes ruleType, IEnumerable<decimal> args)
        {
            switch (ruleType)
            {
                case RuleTypes.Addition:
                case RuleTypes.BaseAddition:
                    {
                        return args.Sum();
                    }

                case RuleTypes.Multiplication:
                case RuleTypes.BaseMultiplication:
                    {
                        return args.Product();
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Тип правила {0} не поддерживается", ruleType));
                    }
            }
        }
    }
}
