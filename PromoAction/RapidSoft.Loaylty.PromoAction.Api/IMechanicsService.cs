namespace RapidSoft.Loaylty.PromoAction.Api
{
    using System.Collections.Generic;
    using System.ServiceModel;

    using Monitoring;

    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;

    /// <summary>
    /// Интерфейс-контракт сервиса расчета значения механики
    /// </summary>
    [ServiceContract]
    public interface IMechanicsService : ISupportService
    {
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
        [OperationContract]
        CalculateResult CalculateSingleValue(string ruleDomainName, decimal initialNumber, IDictionary<string, string> context);

        /// <summary>
        /// Метод расчета коэффициентов домена. Аналогичен <see cref="CalculateSingleValue"/>, но без входного числа
        /// </summary>
        /// <param name="ruleDomainName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        FactorsResult CalculateFactors(string ruleDomainName, IDictionary<string, string> context);
        
        /// <summary>
        /// Метод для генерации условного оператора на языке T-SQL.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Результат вычимсления
        /// </returns>
        [OperationContract]
        GenerateResult GenerateSql(GenerateSqlParameters parameters);
    }
}