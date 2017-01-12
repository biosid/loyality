namespace RapidSoft.VTB24.BankConnector.DataSource.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.WsClients.AdminMechanicsService;
    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource.Interface;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;

    public class AdminMechanicsServiceProvider : IAdminMechanicsServiceProvider
    {
	    private readonly IAdminMechanicsService adminMechanicsService;

		public AdminMechanicsServiceProvider(IAdminMechanicsService adminMechanicsService)
		{
			this.adminMechanicsService = adminMechanicsService;
		}

        IList<Rule> IAdminMechanicsServiceProvider.GetPromoActions(DateTime dateTimeFrom, bool activeOnly, params ApproveStatus[] approveStatuses)
        {
            var status = activeOnly ? (RuleStatuses?)RuleStatuses.Active : null;

            RulesResult rulesResult;
            {
                var parameters = new GetRulesParameters
                                     {
                                         DateTimeFrom = dateTimeFrom,
                                         Status = status,
                                         CalcTotalCount = true,
                                         UserId = ConfigHelper.VtbSystemUser,
                                         ApproveStatuses = approveStatuses
                                     };
                rulesResult =
                    adminMechanicsService.GetPromoActions(parameters);
            }

            if (!rulesResult.Success)
            {
                throw new Exception("Не удалось получить промоакции: " + rulesResult.ResultDescription);
            }

            var rules = rulesResult.Rules;
            
            while (rulesResult.TotalCount > rules.Length)
            {
                var parameters = new GetRulesParameters
                                     {
                                         DateTimeFrom = dateTimeFrom,
                                         Status = status,
                                         CalcTotalCount = true,
                                         CountSkip = rules.Length,
                                         UserId = ConfigHelper.VtbSystemUser
                                     };
                var rulesLoopResult =
                    adminMechanicsService.GetPromoActions(parameters);

                if (!rulesLoopResult.Success)
                {
                    throw new Exception("Не удалось получить промоакции: " + rulesLoopResult.ResultDescription);
                }

                rules = rules.Union(rulesLoopResult.Rules).ToArray();
            }

            return rules;
        }

        public void SetPromoActionsStatus(IList<PromoActionResponse> promoActionResponses)
        {
            if (promoActionResponses == null || promoActionResponses.Count == 0)
            {
                return;
            }

            var approves = promoActionResponses.Select(this.ToApprove).ToArray();

            var result =
                    adminMechanicsService.SetRuleApproved(approves, ConfigHelper.VtbSystemUser);

            if (!result.Success)
            {
                throw new Exception("Ошибка установки статусов утверждения: " + result.ResultDescription);
            }
        }

        private Approve ToApprove(PromoActionResponse response)
        {
            bool isApproved;
            string reason;
            switch (response.Status)
            {
                case 1:
                    {
                        isApproved = true;
                        reason = "Кампания успешно загружена";
                        break;
                    }

                case 2:
                    {
                        isApproved = false;
                        reason =
                            "Кампания не загружена, кампания с указанным идентификатором находится в архиве или удалена";
                        break;
                    }

                case 3:
                    {
                        isApproved = false;
                        reason =
                            "Кампания не загружена, даты проведения кампании являются некорректными";
                        break;
                    }

                default:
                    {
                        var mess = string.Format("Ответный статус {0} не поддерживается", response.Status);
                        throw new NotSupportedException(mess);
                    }
            }

            var retVal = new Approve { RuleId = response.PromoId, IsApproved = isApproved, Reason = reason };
            return retVal;
        }
    }
}