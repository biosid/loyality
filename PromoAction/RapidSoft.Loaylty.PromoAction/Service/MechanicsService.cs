namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Monitoring;

    using RapidSoft.Extensions;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.Logging.Wcf;
    using RapidSoft.Loaylty.PromoAction.Api;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Exceptions;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Tracer;
    using RapidSoft.Loaylty.PromoAction.Wcf;

    /// <summary>
    /// Реализация сервиса расчета механики
    /// </summary>
    [DbContextBehavior, LoggingBehavior]
    public class MechanicsService : SupportService, IMechanicsService, IServiceInfo
    {
        private readonly ILog log = LogManager.GetLogger(typeof(MechanicsService));

        /// <summary>
        /// Репозиторий правил.
        /// </summary>
        private readonly IRuleRepository ruleRepository;

        /// <summary>
        /// Репозиторий доменов правил.
        /// </summary>
        private readonly IRuleDomainRepository ruleDomainRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MechanicsService"/> class.
        /// </summary>
        public MechanicsService()
        {
            this.ruleRepository = new RuleRepository();
            this.ruleDomainRepository = new RuleDomainRepository();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MechanicsService"/> class.
        /// </summary>
        /// <param name="ruleRepository">
        /// Репозиторий правил.
        /// </param>
        /// <param name="ruleDomainRepository">
        /// Репозиторий доменов правил.
        /// </param>
        public MechanicsService(IRuleRepository ruleRepository, IRuleDomainRepository ruleDomainRepository)
        {
            ruleRepository.ThrowIfNull("ruleRepository");
            ruleDomainRepository.ThrowIfNull("ruleDomainRepository");

            this.ruleRepository = ruleRepository;
            this.ruleDomainRepository = ruleDomainRepository;
        }

        /// <summary>
        /// Метод расчета одного значения с учетом лимитного ограничения.
        /// </summary>
        /// <param name="ruleDomainName">
        /// Имя домена правил, которые надо применить к исходному числу
        /// </param>
        /// <param name="initialNumber">
        /// Число, к которому необходимо применить правила из указанного домена правил
        /// </param>
        /// <param name="context">
        /// Контекст для вычисления, коллекция пар «переменная-значение».
        /// </param>
        /// <returns>
        /// Результат выполнения метода, см <see cref="CalculateResult"/>
        /// </returns>
        public CalculateResult CalculateSingleValue(string ruleDomainName, decimal initialNumber, IDictionary<string, string> context)
        {
            try
            {
                var domain = this.ruleDomainRepository.GetByName(ruleDomainName);

                if (domain == null)
                {
                    log.InfoFormat("Домен правил \"{0}\" не найден", ruleDomainName);
                    return CalculateResult.BuildFail(initialNumber, "Домен правил не найден");
                }

                var resultWithoutLimits = this.CalculateSingleValueWithoutLimit(ruleDomainName, initialNumber, context);

                if (!resultWithoutLimits.Success)
                {
                    return resultWithoutLimits;
                }

                var calculator = CalculatorFactory.GetLimitCalculator(domain);

                var resultNumber = calculator.Calculate(initialNumber, resultWithoutLimits.PromoResult);

                resultWithoutLimits.PromoResult = resultNumber;

                return resultWithoutLimits;
            }
            catch (Exception ex)
            {
                log.Error("Не обработанная ошибка", ex);
                var retVal = ServiceOperationResult.BuildErrorResult<CalculateResult>(ex);
                retVal.RuleApplyStatus = RuleApplyStatuses.RulesNotExecuted;
                return retVal;
            }
        }

        public FactorsResult CalculateFactors(string ruleDomainName, IDictionary<string, string> context)
        {
            ITracer tracer = new Tracer();
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                var rules = this.ruleRepository.GetActualRulesByRuleDomainName(ruleDomainName) ?? new Rule[0];

                var defaultRules = this.GetRulesByDomainDefaultFactors(ruleDomainName);

                rules = rules.Concat(defaultRules).ToList();

                if (rules.Count == 0)
                {
                    log.InfoFormat("Домен правил \"{0}\" или правила в домене не найдены", ruleDomainName);
                    return FactorsResult.BuildFail("Домен правил или правила в домене не найдены");
                }

                var settings = new EvaluationSettings(tracer, context);

                var calculator = CalculatorFactory.GetCalculator(settings);

                var results = calculator.Calculate(rules);

                if (results.Any(x => x.Code == EvaluateResultCode.ConvertibleToSQL))
                {
                    const string Mess = "Вычисление коэффициентов не возможно, в контексте не достаточно данных";
                    log.InfoFormat(Mess, ruleDomainName);
                    return FactorsResult.BuildFail(Mess);
                }

                bool isFound;

                var baseMultiplicationFactor = results.GetRulesFactor(RuleTypes.BaseMultiplication, tracer, out isFound);
                var isBaseApplied = isFound;

                var baseAdditionFactor = results.GetRulesFactor(RuleTypes.BaseAddition, tracer, out isFound);
                isBaseApplied = isBaseApplied || isFound;

                var multiplicationFactor = results.GetRulesFactor(RuleTypes.Multiplication, tracer, out isFound);
                var isNotBaseApplied = isFound;

                var additionFactor = results.GetRulesFactor(RuleTypes.Addition, tracer, out isFound);
                isNotBaseApplied = isNotBaseApplied || isFound;

                return FactorsResult.BuildSuccess(
                    baseMultiplicationFactor,
                    baseAdditionFactor,
                    multiplicationFactor,
                    additionFactor,
                    isBaseApplied,
                    isNotBaseApplied,
                    tracer.GetMessages());
            }
            catch (RuleEvaluationException ex)
            {
                log.Error("Ошибка вычисления предиката", ex);
                var retVal = ServiceOperationResult.BuildErrorResult<FactorsResult>(ex);
                return retVal;
            }
            catch (Exception ex)
            {
                log.Error("Не обработанная ошибка", ex);
                var retVal = ServiceOperationResult.BuildErrorResult<FactorsResult>(ex);
                return retVal;
            }
        }

        /// <summary>
        /// Метод для генерации условного оператора на языке T-SQL.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат вычисления
        /// </returns>
        public GenerateResult GenerateSql(GenerateSqlParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters");

                var domain = this.ruleDomainRepository.GetByName(parameters.RuleDomainName);

                if (domain == null)
                {
                    log.InfoFormat("Домен правил \"{0}\" не найден", parameters.RuleDomainName);
                    return GenerateResult.BuildFail("Домен правил не найден");
                }

                var resultWithoutLimits = this.GenerateSqlWithoutLimit(parameters.RuleDomainName, parameters.Context, parameters.Aliases);
                if (!resultWithoutLimits.Success)
                {
                    return GenerateResult.BuildFail(resultWithoutLimits);
                }

                var calculator = CalculatorFactory.GetLimitCalculator(domain);

                var baseSql = calculator.GenerateBaseSql(
                    parameters.InitialNumberAlias,
                    resultWithoutLimits.BaseMultiplicationSql,
                    resultWithoutLimits.BaseAdditionSql);

                var limitSql = calculator.GenerateLimitedSql(
                    parameters.InitialNumberAlias,
                    resultWithoutLimits.BaseMultiplicationSql,
                    resultWithoutLimits.BaseAdditionSql,
                    resultWithoutLimits.MultiplicationSql,
                    resultWithoutLimits.AdditionSql);

                return GenerateResult.BuildSuccess(baseSql, limitSql);
            }
            catch (Exception ex)
            {
                log.Error("Не обработанная ошибка", ex);
                return ServiceOperationResult.BuildErrorResult<GenerateResult>(ex);
            }
        }

        #region IServiceInfo Members

        /// <summary>
        /// Метод проверки связнанных ресурсов.
        /// </summary>
        public void Ping()
        {
            var errorStr = new StringBuilder();

            try
            {
                this.ruleRepository.GetActualRulesByRuleDomainName(string.Empty);
            }
            catch (Exception e)
            {
                log.Error("База данных компонента не доступна", e);
                errorStr.AppendLine("База данных компонента не доступна");
            }

            if (errorStr.Length > 0)
            {
                throw new Exception(errorStr.ToString());
            }
        }

        /// <summary>
        /// Метод получения версии компонента.
        /// </summary>
        /// <returns>
        /// The <see cref="Version"/>.
        /// </returns>
        public Version GetServiceVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
        #endregion

        /// <summary>
        /// Метод расчета одного значения без учета лимитного ограничения.
        /// </summary>
        /// <param name="ruleDomainName">
        /// Имя домена правил, которые надо применить к исходному числу
        /// </param>
        /// <param name="initialNumber">
        /// Число, к которому необходимо применить правила из указанного домена правил
        /// </param>
        /// <param name="context">
        /// Контекст для вычисления, коллекция пар «переменная-значение».
        /// </param>
        /// <returns>
        /// Результат выполнения метода, см <see cref="CalculateResult"/>
        /// </returns>
        private CalculateResult CalculateSingleValueWithoutLimit(string ruleDomainName, decimal initialNumber, IDictionary<string, string> context)
        {
            var factors = this.CalculateFactors(ruleDomainName, context);

            if (!factors.Success)
            {
                return CalculateResult.BuildFail(initialNumber, factors.ResultDescription);
            }

            var retBaseVal = (initialNumber * factors.BaseMultiplicationFactor) + factors.BaseAdditionFactor;
            var isBaseApplied = factors.IsBaseApplied;

            var promoResult = (retBaseVal * factors.MultiplicationFactor) + factors.AdditionFactor;
            var isNotBaseApplied = factors.IsNotBaseApplied;

            return CalculateResult.BuildSuccess(retBaseVal, promoResult, isBaseApplied, isNotBaseApplied, factors.TraceMessages);
        }

        /// <summary>
        /// Метод для вычисления коэффициентов без учета лимитного ограничения, используя <paramref name="context"/>, 
        /// и генерации условного оператора на языке T-SQL, используя <paramref name="aliases"/>.
        /// Если предикат можно вычислить используя значения из <paramref name="context"/>
        /// </summary>
        /// <param name="ruleDomainName">
        /// Имя домена правил, которое необходимо преобразовать.
        /// </param>
        /// <param name="context">
        /// Контекст для вычисления, коллекция пар «переменная-значение».
        /// </param>
        /// <param name="aliases">
        /// Контекст для вычисления, коллекция пар «переменная-алиас столбца».
        /// </param>
        /// <returns>
        /// Результат вычимсления
        /// </returns>
        public GenerateDetailedResult GenerateSqlWithoutLimit(string ruleDomainName, IDictionary<string, string> context, IDictionary<string, string> aliases)
        {
            var tracer = new Tracer();
            try
            {
                var rules = this.ruleRepository.GetActualRulesByRuleDomainName(ruleDomainName) ?? new Rule[0];

                var defaultRules = this.GetRulesByDomainDefaultFactors(ruleDomainName);

                rules = rules.Concat(defaultRules).ToList();

                if (rules.Count == 0)
                {
                    log.InfoFormat("Домен правил \"{0}\" или правила в домене не найдены", ruleDomainName);
                    return GenerateDetailedResult.BuildFail("Домен правил или правила в домене не найдены");
                }

                var settings = new EvaluationSettings(tracer, context, aliases);

                var calculator = CalculatorFactory.GetCalculator(settings);

                var results = calculator.Calculate(rules);

                return GenerateDetailedResult.BuildSuccess(
                    results.ConvertToSql(RuleTypes.BaseMultiplication),
                    results.ConvertToSql(RuleTypes.BaseAddition),
                    results.ConvertToSql(RuleTypes.Multiplication),
                    results.ConvertToSql(RuleTypes.Addition),
                    tracer.GetMessages());
            }
            catch (RuleEvaluationException ex)
            {
                log.Error("Ошибка вычисления предиката", ex);
                return ServiceOperationResult.BuildErrorResult<GenerateDetailedResult>(ex);
            }
            catch (Exception ex)
            {
                log.Error("Не обработанная ошибка", ex);
                return ServiceOperationResult.BuildErrorResult<GenerateDetailedResult>(ex);
            }
        }

        private IEnumerable<Rule> GetRulesByDomainDefaultFactors(string ruleDomainName)
        {
            var domain = this.ruleDomainRepository.GetByName(ruleDomainName);

            if (domain == null)
            {
                var mess = string.Format("Домен правил \"{0}\" не найден", ruleDomainName);
                log.Info(mess);
                throw new OperationException(ResultCodes.UNKNOWN_ERROR, mess);
            }

            var baseAddRule = Rule.BuildDefaultRule(domain.DefaultBaseAdditionFactor, RuleTypes.BaseAddition, domain);
            yield return baseAddRule;

            var baseMulRule = Rule.BuildDefaultRule(domain.DefaultBaseMultiplicationFactor, RuleTypes.BaseMultiplication, domain);
            yield return baseMulRule;
        }
    }
}