namespace RapidSoft.Loaylty.PromoAction.Mechanics
{
    using System;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Calculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.LimitCalculators;

    /// <summary>
    /// Фабрика композитного калькулятор групп правил.
    /// </summary>
    public static class CalculatorFactory
    {
        /// <summary>
        /// Создает композитный калькулятор групп правил.
        /// </summary>
        /// <param name="settings">
        /// Настройки вычисления.
        /// </param>
        /// <returns>
        /// Композитный калькулятор групп правил.
        /// </returns>
        public static CompositeCalculator GetCalculator(EvaluationSettings settings)
        {
            settings.ThrowIfNull("settings");

            var baseAdd = new BaseRulesCalculator(RuleTypes.BaseAddition, settings);
            var baseMul = new BaseRulesCalculator(RuleTypes.BaseMultiplication, settings);

            var exclusiveAdd = new ExclusiveCalculator(RuleTypes.Addition, settings);
            var exclusiveMul = new ExclusiveCalculator(RuleTypes.Multiplication, settings);

            return new CompositeCalculator(baseAdd, baseMul, exclusiveAdd, exclusiveMul);
        }

        public static ILimitCalculator GetLimitCalculator(RuleDomain ruleDomain)
        {
            ruleDomain.ThrowIfNull("ruleDomain");

            var stop = GetStopLimitCalculator(ruleDomain);

            switch (ruleDomain.LimitType)
            {
                case LimitTypes.Fixed:
                    {
                        return new FixedLimitCalculator(ruleDomain.LimitFactor, stop);
                    }

                case LimitTypes.Percent:
                    {
                        return new ProcentLimitCalculator(ruleDomain.LimitFactor, stop);
                    }

                default:
                    {
                        var mess = string.Format("Тип вычисления лимитного значения {0} не поддреживается", ruleDomain.LimitType);
                        throw new NotSupportedException(mess);
                    }
            }
        }

        public static IStopLimitCalculator GetStopLimitCalculator(RuleDomain ruleDomain)
        {
            ruleDomain.ThrowIfNull("ruleDomain");

            switch (ruleDomain.StopLimitType)
            {
                case LimitTypes.Fixed:
                    {
                        return new FixedStopLimitCalculator(ruleDomain.StopLimitFactor);
                    }

                case LimitTypes.Percent:
                    {
                        return new ProcentStopLimitCalculator(ruleDomain.StopLimitFactor);
                    }

                default:
                    {
                        var mess = string.Format("Тип вычисления лимитного значения {0} не поддреживается", ruleDomain.StopLimitType);
                        throw new NotSupportedException(mess);
                    }
            }
        }
    }
}