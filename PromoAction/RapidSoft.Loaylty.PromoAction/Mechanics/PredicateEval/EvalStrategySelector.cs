namespace RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    /// <summary>
    /// Фабрика выбора стратегий вычисления.
    /// </summary>
    public class EvalStrategySelector : IEvalStrategySelector
    {
        /// <summary>
        /// Сопоставление операций и их строкового представления.
        /// </summary>
        private static readonly Dictionary<equationOperator, string> EquationOperators =
            new Dictionary<equationOperator, string>
                    {
                        { equationOperator.em, " IS NULL" },
                        { equationOperator.nem, " IS NOT NULL" },
                        { equationOperator.eq, "=" },
                        { equationOperator.noteq, "!=" },
                        { equationOperator.gt, ">" },
                        { equationOperator.lt, "<" },
                        { equationOperator.lteq, "<=" },
                        { equationOperator.gteq, ">=" },
                        { equationOperator.like, " LIKE " }
                    };

        /// <summary>
        /// Определитель значения переменной или строкового литерала.
        /// </summary>
        private readonly IVariableResolver variableResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvalStrategySelector"/> class.
        /// </summary>
        /// <param name="variableResolver">
        /// Определитель значения переменной или строкового литерала.
        /// </param>
        public EvalStrategySelector(IVariableResolver variableResolver)
        {
            variableResolver.ThrowIfNull("variableResolver");

            this.variableResolver = variableResolver;
        }

        /// <summary>
        /// Создает страгерию вычисления конктерного предиката (<see cref="filter"/>).
        /// </summary>
        /// <param name="filter">
        /// Рассчитываемый предикат.
        /// </param>
        /// <param name="settings">
        /// Настройки вычисления.
        /// </param>
        /// <returns>
        /// Страгерия трансформирующего вычисления.
        /// </returns>
        public IEquationEval SelectEvalStrategy(filter filter, Settings settings)
        {
            var union = filter.Item as union;
            if (union != null)
            {
                return this.SelectEvalStrategy(union);
            }

            var equation = filter.Item as equation;
            if (equation != null)
            {
                return this.SelectEvalStrategy(equation);
            }

            throw new NotSupportedException(string.Format("Не возможно вычислить объект типа {0} как предикат", filter.Item.GetType().Name));
        }

        /// <summary>
        /// Создание конвертера объединения.
        /// </summary>
        /// <param name="unionType">
        /// Тип объединения.
        /// </param>
        /// <param name="operands">
        /// Коллекция операндов.
        /// </param>
        /// <returns>
        /// Конвертер объединения.
        /// </returns>
        public IPredicateSqlConverter CreateUnionConverter(string unionType, IEnumerable<EvaluateResult> operands)
        {
            var @operator = this.SelectUnionOperator(unionType);

            return new UnionConverter(@operator, operands);
        }

        /// <summary>
        /// Создание конвертера уравнения.
        /// </summary>
        /// <param name="equationOperator">
        /// Тип уравнения.
        /// </param>
        /// <param name="values">
        /// Переменные уравнения.
        /// </param>
        /// <returns>
        /// Конвертер уравнения.
        /// </returns>
        public IPredicateSqlConverter CreateEquationConverter(equationOperator equationOperator, params VariableValue[] values)
        {
            var @operator = this.SelectEquationOperator(equationOperator);

            switch (equationOperator)
            {
                case equationOperator.em:
                case equationOperator.nem:
                    {
                        if (values.Length != 1)
                        {
                            throw new InvalidPredicateFormatException("Уравнение с одной переменной должно содержать только 1 переменную/литерал");
                        }

                        var operand = this.Convert(values[0]);
                        return new OneOperandEquationConverter(operand, @operator);
                    }

                case equationOperator.eq:
                case equationOperator.noteq:
                case equationOperator.gt:
                case equationOperator.gteq:
                case equationOperator.lt:
                case equationOperator.lteq:
                    {
                        if (values.Length != 2)
                        {
                            throw new InvalidPredicateFormatException("Уравнение с двумя переменными должно содержать только 2 переменных/литерала");
                        }

                        var operand1 = this.Convert(values[0]);
                        var operand2 = this.Convert(values[1]);

                        return new TwoOperandEquationConverter(operand1, @operator, operand2);
                    }

                case equationOperator.like:
                    {
                        if (values.Length != 2)
                        {
                            throw new InvalidPredicateFormatException("Уравнение проверки вхождения строки должно содержать 2 переменных/литерала");
                        }

                        var operand1 = this.Convert(values[0]);
                        var operand2 = this.Convert(values[1]);

                        return new LikeConverter(operand1, @operator, operand2);
                    }

                case equationOperator.cn:
                case equationOperator.dncn:
                case equationOperator.notnull:
                case equationOperator.@null:
                case equationOperator.between:
                    {
                        // NOTE: Контрол создания фильтра/предиката не поддерживает эти операции.
                        throw new NotSupportedException(string.Format("Операция {0} не поддерживается", equationOperator));
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Операция {0} не поддерживается", equationOperator));
                    }
            }
        }
        
        /// <summary>
        /// Конвертирует оператор уравнения в строку.
        /// </summary>
        /// <param name="equationOperator">
        /// Исходный оператор уравнения.
        /// </param>
        /// <returns>
        /// Строковое представление оператора.
        /// </returns>
        public string SelectEquationOperator(equationOperator equationOperator)
        {
            var @operator = EquationOperators.FirstOrDefault(x => x.Key == equationOperator).Value;

            if (@operator != null)
            {
                return @operator;
            }

            throw new NotSupportedException(string.Format("Операция {0} не поддерживается", equationOperator));
        }

        /// <summary>
        /// Конвертирует оператор объединения в строку.
        /// </summary>
        /// <param name="unionType">
        /// Исходный оператор объединения.
        /// </param>
        /// <returns>
        /// Строковое представление объединения.
        /// </returns>
        public string SelectUnionOperator(string unionType)
        {
            switch (unionType)
            {
                case "and":
                    {
                        return " AND ";
                    }

                case "or":
                    {
                        return " OR ";
                    }

                default:
                    {
                        // NOTE: Дополнительные ISqlConvertible реализуются по мере необходимости.
                        throw new NotSupportedException(string.Format("Объединение {0} не поддерживается", unionType));
                    }
            }
        }

        /// <summary>
        /// Создает страгерию вычисления конктерного объединения (<see cref="union"/>).
        /// </summary>
        /// <param name="union">
        /// Рассчитываемое объединение.
        /// </param>
        /// <returns>
        /// Страгерия трансформирующего вычисления.
        /// </returns>
        private IEquationEval SelectEvalStrategy(union union)
        {
            var operands = this.CollectOperands(union, this.SelectEvalStrategy, this.SelectEvalStrategy);

            switch (union.type)
            {
                case "and":
                    {
                        return new AndEval(operands, this);
                    }

                case "or":
                    {
                        return new OrEval(operands, this);
                    }

                default:
                    {
                        // NOTE: Дополнительные ISqlConvertible реализуются по мере необходимости.
                        throw new NotSupportedException(string.Format("Объединение {0} не поддерживается", union.type));
                    }
            }
        }

        /// <summary>
        /// Создает страгерию вычисления конктерного уравнения (<see cref="equation"/>).
        /// </summary>
        /// <param name="equation">
        /// Рассчитываемое уравнение.
        /// </param>
        /// <returns>
        /// Страгерия трансформирующего вычисления.
        /// </returns>
        private IEquationEval SelectEvalStrategy(equation equation)
        {
            var values = equation.value.Select(this.variableResolver.ResolveVariable).ToArray();

            // NOTE: В текущий момент нету специальных переменных
            // if (values.Any(x => x.Status == VariableStatuses.SpecialValue))
            // {
            //	return this.SelectSpecialEvalStrategy(equation.@operator, values);
            // }
            switch (equation.@operator)
            {
                case equationOperator.em:
                    {
                        return new EmEquation(values, this);
                    }

                case equationOperator.nem:
                    {
                        return new NemEquation(values, this);
                    }

                case equationOperator.eq:
                    {
                        return new EqEquation(values, this);
                    }

                case equationOperator.noteq:
                    {
                        return new NotEqEquation(values, this);
                    }

                case equationOperator.gt:
                    {
                        return new GtEquation(values, this);
                    }

                case equationOperator.lt:
                    {
                        return new LtEquation(values, this);
                    }

                case equationOperator.lteq:
                    {
                        return new LtEqEquation(values, this);
                    }

                case equationOperator.gteq:
                    {
                        return new GtEquation(values, this);
                    }

                case equationOperator.like:
                    {
                        return new LikeEquation(values, this);
                    }

                case equationOperator.cn:
                    {
                        return new ContainEquation(values);
                    }

                case equationOperator.dncn:
                    {
                        return new NotContainEquation(values);
                    }

                case equationOperator.notnull:
                case equationOperator.@null:
                case equationOperator.between:
                    {
                        // NOTE: Контрол создания фильтра/предиката не поддерживает эти операции.
                        throw new NotSupportedException(
                            string.Format(
                                "Операция {0} не поддерживается для простых (число, строка, логическое и дата с времененм) значений",
                                equation.@operator));
                    }

                default:
                    {
                        throw new NotSupportedException(
                            string.Format(
                                "Операция {0} не поддерживается для простых (число, строка, логическое и дата с времененм) значений",
                                equation.@operator));
                    }
            }
        }

        /// <summary>
        /// Выполняет "сбор" операндов объединения с выбором стратегии, так как опердандом объединения может быть уравнение или другое объединение.
        /// </summary>
        /// <param name="union">
        /// Анализируемое объединение.
        /// </param>
        /// <param name="selectEquationStrategy">
        /// Функция выбора страгении вычисления уравнения.
        /// </param>
        /// <param name="selectUnionStrategy">
        /// Функция выбора страгении вычисления объединения.
        /// </param>
        /// <typeparam name="T">
        /// Тип выбранной стратегии.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private IEnumerable<T> CollectOperands<T>(union union, Func<equation, T> selectEquationStrategy, Func<union, T> selectUnionStrategy)
        {
            IEnumerable<T> equationArgs = null;
            IEnumerable<T> unionArgs = null;

            if (union.equation != null)
            {
                equationArgs = union.equation.Select(selectEquationStrategy);
            }

            if (union.union1 != null)
            {
                unionArgs = union.union1.Select(selectUnionStrategy);
            }

            var operands = equationArgs.SafeUnion(unionArgs);
            return operands;
        }

        /// <summary>
        /// Конвертирует значение переменной в строковое представление.
        /// </summary>
        /// <param name="value">
        /// Конвертируемая переменная.
        /// </param>
        /// <returns>
        /// Строковое представление значения переменной.
        /// </returns>
        private string Convert(VariableValue value)
        {
            switch (value.Status)
            {
                case VariableStatuses.SimplyValue:
                    {
                        return this.ConvertSimplyValueToSql(value.Value);
                    }

                case VariableStatuses.SqlColumn:
                    {
                        return value.Value as string;
                    }

                default:
                    {
                        throw new NotSupportedException(string.Format("Переменную со статусом {0} нельзя преобразовать в SQL", value.Status));
                    }
            }
        }

        /// <summary>
        /// Конвертер простых типов в строковое представление MS SQL Server.
        /// </summary>
        /// <param name="value">
        /// Конвертируемое значение.
        /// </param>
        /// <returns>
        /// Строковое представление конвертируемого значения.
        /// </returns>
        private string ConvertSimplyValueToSql(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            if (value is decimal)
            {
                return ((decimal)value).ToString(CultureInfo.InvariantCulture);
            }

            if (value is long)
            {
                return ((long)value).ToString(CultureInfo.InvariantCulture);
            }

            if (value is string)
            {
                return "'" + value + "'";
            }

            if (value is bool)
            {
                return (bool)value ? "1" : "0";
            }

            if (value is DateTime)
            {
                var dateTime = (DateTime)value;
                var sqlDateTimeStr = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                return "CONVERT(datetime,'" + sqlDateTimeStr + "',126)";
            }

            throw new NotSupportedException(string.Format("Переменная типа {0} нельзя преобразовать в SQL", value.GetType().Name));
        }
    }
}