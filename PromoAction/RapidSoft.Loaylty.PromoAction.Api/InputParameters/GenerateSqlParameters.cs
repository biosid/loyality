namespace RapidSoft.Loaylty.PromoAction.Api.InputParameters
{
    using System.Collections.Generic;

    public class GenerateSqlParameters
    {
        public GenerateSqlParameters()
        {
        }

        public GenerateSqlParameters(string ruleDomainName, string initialNumberAlias, IDictionary<string, string> context, IDictionary<string, string> aliases)
        {
            this.RuleDomainName = ruleDomainName;
            this.InitialNumberAlias = initialNumberAlias;
            this.Context = context;
            this.Aliases = aliases;
        }

        /// <summary>
        /// Имя домена правил.
        /// </summary>
        public string RuleDomainName { get; set; }

        /// <summary>
        /// Алиас столбца исходного числа.
        /// </summary>
        public string InitialNumberAlias { get; set; }

        /// <summary>
        /// Контекст для вычисления, коллекция пар «переменная-значение».
        /// </summary>
        public IDictionary<string, string> Context { get; set; }

        /// <summary>
        /// Контекст для вычисления, коллекция пар «переменная-алиас столбца».
        /// </summary>
        public IDictionary<string, string> Aliases { get; set; }
    }
}