namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading;

    using API.Entities;
    using API.OutputResults;

    using DataSources;

    using Extensions;

    using Fake;
    using Interfaces;
    using PromoAction.WsClients.MechanicsService;
    using RapidSoft.Loaylty.Logging;

    using Settings;

    public class MechanicsProvider : IMechanicsProvider
    {
        private const string PriceColumnAlias = "p.PriceRUR";
        private const string BasePriceColumnAlias = "COALESCE(p.BasePriceRUR, p.PriceRUR)";
        private const string CalcProdPrice = "Расчёт цены вознаграждения";
        private const string CalcProdPriceOnline = "Расчёт цены вознаграждения онлайн партнёра";
        private const string CalcProdDeliveryPrice = "Расчёт цены доставки";

        private static DateTime cacheDate;
        private static ConcurrentDictionary<string, GenerateResult> cache;

        private readonly IMechanicServiceClient mechanicServiceClient;
        private readonly ILog log = LogManager.GetLogger(typeof(MechanicsProvider));

        public MechanicsProvider()
        {
            mechanicServiceClient = new MechanicServiceClientAdapter();
        }

        public MechanicsProvider(IMechanicServiceClient mechanicServiceClient)
        {
            this.mechanicServiceClient = mechanicServiceClient ?? new MechanicServiceClientAdapter();
        }

        #region IMechanicsProvider Members

        public GenerateResult GetPriceSql(Dictionary<string, string> clientContext)
        {
            var normalized = NormalizeClientContext(clientContext);            

            try
            {
                var domainName = ConfigurationManager.AppSettings.Get("SearchProductsMechanicDomainName") ?? CalcProdPrice;

                var param = new GenerateSqlParameters
                            {
                                RuleDomainName = domainName,
                                InitialNumberAlias = "{0}",
                                Context = normalized
                            };

                var result = GenerateSqlResult(param);
                
                result.ActionSql = string.Format(result.ActionSql, PriceColumnAlias);
                result.BaseSql = string.Format(result.BaseSql, BasePriceColumnAlias);
                
                return result;
            }
            catch (Exception e)
            {
                throw new OperationException("Не удалось расчитать цену", e)
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }
        }

        public CalculateResult CalculateProductPrice(Dictionary<string, string> clientContext, decimal priceRUR, Product product)
        {
            return CalcSingleValue(clientContext, priceRUR, product, GetProductPriceDomainName());
        }

        public CalculatedPrice CalculateDeliveryPrice(Dictionary<string, string> clientContext, decimal deliveryPriceRur, int partnerId)
        {
            clientContext = BuildProductContextForDelivery(clientContext, partnerId);

            var factors = CalculateFactors(clientContext, GetDeliveryPriceDomainName());

            return new CalculatedPrice()
            {
                PartnerId = partnerId,
                Price = new Price()
                {
                    Rur = deliveryPriceRur,
                    Bonus = CalcBonusPrice(factors, deliveryPriceRur)
                }
            };
        }

        public CalculatedPrice[] CalculateDeliveryPrices(Dictionary<string, string> clientContext, decimal[] deliveryPrices, int partnerId)
        {
            var res = new List<CalculatedPrice>();

            clientContext = BuildProductContextForDelivery(clientContext, partnerId);

            var factors = CalculateFactors(clientContext, GetDeliveryPriceDomainName());

            foreach (var deliveryPrice in deliveryPrices)
            {
                res.Add(new CalculatedPrice()
                    {
                        PartnerId = partnerId,
                        Price = new Price()
                            {
                                Rur = deliveryPrice,
                                Bonus = CalcBonusPrice(factors, deliveryPrice)
                            }
                    });
            }

            return res.ToArray();
        }

        public FactorsResult GetOnlineProductFactors(Dictionary<string, string> clientContext)
        {
            return CalculateFactors(clientContext, CalcProdPriceOnline);
        }

        #endregion

        #region Methods

        public void ClearCache()
        {
            cacheDate = DateTime.MinValue;
        }

        private decimal CalcBonusPrice(FactorsResult factors, decimal deliveryPrice)
        {
            var price = (((deliveryPrice * factors.BaseMultiplicationFactor) + factors.BaseAdditionFactor) * factors.MultiplicationFactor) + factors.AdditionFactor;
            return price.Round();
        }

        private CalculateResult CalcSingleValue(Dictionary<string, string> clientContext, decimal priceRUR, Product product, string domain)
        {
            clientContext = BuildProductContext(clientContext, product);

            var actualPrice = CalcSingleValue(clientContext, priceRUR, domain);

            if (!actualPrice.Success)
            {
                throw new OperationException("Не удалось вычислить цену в баллах")
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }

            return actualPrice;
        }

        private Dictionary<string, string> BuildProductContext(Dictionary<string, string> clientContext, Product product)
        {
            var res = new ClientContextBuilder<Product>(ProductsDataSource.ProductColumns).GetProductContext(product, "p.");

            if (clientContext == null)
            {
                clientContext = new Dictionary<string, string>();
            }

            clientContext = clientContext.Union(res).ToDictionary(k => k.Key, v => v.Value);
            return clientContext;
        }

        private Dictionary<string, string> BuildProductContextForDelivery(Dictionary<string, string> clientContext, int partnerId)
        {
            if (clientContext == null)
            {
                clientContext = new Dictionary<string, string>();
            }

            clientContext.Add("p.PartnerId", partnerId.ToString());
            return clientContext;
        }

        private string GetDeliveryPriceDomainName()
        {
            return ConfigurationManager.AppSettings.Get("SearchProductsDeliveryPriceMechanicDomainName") ?? CalcProdDeliveryPrice;
        }

        private string GetProductPriceDomainName()
        {
            return ConfigurationManager.AppSettings.Get("SearchProductsMechanicDomainName") ?? CalcProdPrice;
        }

        private CalculateResult CalcSingleValue(Dictionary<string, string> clientContext, decimal priceRUR, string domain)
        {
            var normalized = NormalizeClientContext(clientContext);
            CalculateResult calculateResult;

            try
            {
                calculateResult = mechanicServiceClient.CalculateSingleValue(domain, priceRUR, normalized);
            }
            catch (Exception e)
            {
                const string Message = "Не удалось расчитать цену товара";
                throw new OperationException(Message, e)
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }

            if (!calculateResult.Success || calculateResult.RuleApplyStatus == RuleApplyStatuses.RulesNotExecuted)
            {
                var message = string.Format("Сервису механик не удалось применить правила. Success={0} RuleApplyStatus={1}", calculateResult.Success, calculateResult.RuleApplyStatus);
                throw new OperationException(message)
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }

            return calculateResult;
        }

        private Dictionary<string, string> NormalizeClientContext(Dictionary<string, string> clientContext)
        {
            if (clientContext == null)
            {
                return new Dictionary<string, string>
                {
                    {
                        ClientContextParser.AudiencesKey, string.Empty
                    },
                    {
                        ClientContextParser.LocationKladrCodeKey, string.Empty
                    }
                };
            }

            var retVal = new Dictionary<string, string>(clientContext);

            if (retVal.ContainsKey(ClientContextParser.AudiencesKey))
            {
                retVal[ClientContextParser.AudiencesKey] = retVal[ClientContextParser.AudiencesKey] ?? string.Empty;
            }
            else
            {
                retVal.Add(ClientContextParser.AudiencesKey, string.Empty);
            }

            if (retVal.ContainsKey(ClientContextParser.LocationKladrCodeKey))
            {
                retVal[ClientContextParser.LocationKladrCodeKey] = retVal[ClientContextParser.LocationKladrCodeKey] ?? string.Empty;
            }
            else
            {
                retVal.Add(ClientContextParser.LocationKladrCodeKey, string.Empty);
            }

            return retVal;
        }

        private string GetContextKey(Dictionary<string, string> param)
        {
            return string.Format("{0}::{1}", string.Join("_", param.Keys), string.Join("_", param.Values));
        }

        private GenerateResult GenerateSqlResult(GenerateSqlParameters param)
        {
            GenerateResult mechanicsSql;
            var hashKey = string.Format("{0}:{1}:{2}", param.RuleDomainName, param.InitialNumberAlias, GetContextKey(param.Context));

            lock (this)
            {
                if (cacheDate.AddSeconds(ApiSettings.MechanicsCasheSeconds) < DateTime.Now)
                {
                    cache = new ConcurrentDictionary<string, GenerateResult>();
                    cacheDate = DateTime.Now;
                }

                if (cache.ContainsKey(hashKey))
                {
                    mechanicsSql = cache[hashKey];
                    log.DebugFormat(
                        "Для домена \"{0}\" из кеша{3}BaseSql: {1}, ActionSql: {2}",
                        param.RuleDomainName,
                        mechanicsSql.BaseSql,
                        mechanicsSql.ActionSql,
                        Environment.NewLine);
                }
                else
                {
                    mechanicsSql = mechanicServiceClient.GenerateSql(param);

                    cache.TryAdd(hashKey, mechanicsSql);

                    log.DebugFormat(
                        "Для домена \"{0}\" от механик{3}BaseSql: {1}, ActionSql: {2}",
                        param.RuleDomainName,
                        mechanicsSql.BaseSql,
                        mechanicsSql.ActionSql,
                        Environment.NewLine);
                }
            }

            return mechanicsSql;
        }

        private FactorsResult CalculateFactors(Dictionary<string, string> clientContext, string domain)
        {
            var normalized = NormalizeClientContext(clientContext);

            FactorsResult result;

            try
            {
                result = mechanicServiceClient.CalculateFactors(domain, normalized);
            }
            catch (Exception e)
            {
                const string Message = "Не удалось получить коэффициенты механик";
                throw new OperationException(Message, e)
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }

            if (!result.Success)
            {
                var message = string.Format("Сервису механик не удалось применить правила. Success={0}", result.Success);
                throw new OperationException(message)
                {
                    ResultCode = ResultCodes.CAN_NOT_CALC_PRICE
                };
            }

            return result;
        }

        #endregion
    }
}