using System;
using System.Linq;
using Vtb24.Site.Services.BonusPaymentGatewayService;
using Vtb24.Site.Services.BonusPayments.Models.Exceptions;
using Vtb24.Site.Services.BonusPayments.Models.Inputs;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Services.BonusPayments
{
    public class BonusPayments : IBonusPayments
    {
        private static int LoyaltyProgramId
        {
            get { return AppSettingsHelper.Int("points_loyalty_program_id" , 0); }
        }

        private static int ChanelId
        {
            get { return AppSettingsHelper.Int("points_channel_id", 0);  }
        }

        private static string PosId
        {
            get { return AppSettingsHelper.String("points_pos_id", ""); }
        }

        private static string TerminalId
        {
            get { return AppSettingsHelper.String("points_terminal_id", ""); }
        }

        private static string PartnerId
        {
            get { return AppSettingsHelper.String("points_partner_id", "");  }
        }

        public void Charge(ChargeParameters parameters)
        {
            parameters.ValidateAndThrow();

            var account = GetPrimaryBonusAccount(parameters.ClientId);

            if (account == null)
            {
                throw new BonusPointsException(
                    -1,
                    string.Format("У клиента (id: {0}) отсутствует основной бонусный счёт. Невозможно списать бонусы", parameters.ClientId)
                );
            }

            using (var service = new BonusGatewayClient())
            {
                var requestParams = new ApplyWithdrawByAccountRequest
                {
                    PointSum = parameters.Sum,
                    AccountId = account.AccountId,
                    LoyaltyProgramId = LoyaltyProgramId,
                    ChannelId = ChanelId,
                    PosId = PosId,
                    TerminalId = TerminalId,
                    PartnerId = PartnerId,
                    RequestDateTime = DateTime.Now,
                    RequestId = parameters.ChequeNumber,
                    Cheque = new Cheque
                    {
                        ChequeDateTime = parameters.ChequeTime,
                        ChequeDiscount = 0,
                        ChequeNumber = parameters.ChequeNumber,
                        ChequeSum = parameters.Sum,
                        Currency = null,
                        Items = parameters.Items.Select(i=>
                            new ChequeItem
                            {
                                ArticleId = i.Article,
                                ArticleName = i.TrimTitle(50),
                                ItemSum = i.Price,
                                Amount = i.Quantity
                            }
                        ).ToArray(),
                    },

                };

                var response = service.ApplyWithdrawByAccount(requestParams);

                if (response.Status != 0)
                {
                    throw new BonusPointsException(response.Status, response.Error ?? "Без описания");
                }
            }
        }

        private static Account GetPrimaryBonusAccount(string clientId)
        {

            using (var service = new BonusGatewayClient())
            {
                var options = new GetAccountsByClientRequest
                {
                    ClientId = clientId,
                    LoyaltyProgramId = LoyaltyProgramId,
                    PartnerId = PartnerId,
                    PosId = PosId,
                    TerminalId = TerminalId,
                    RequestId = Guid.NewGuid().ToString("N"),
                    RequestDateTime = DateTime.Now
                };
                var response = service.GetAccountsByClient(options);

                if (response.Status != 0)
                {
                    throw new BonusPointsException(response.Status, response.Error ?? "Без описания");
                }

                return response.Accounts.FirstOrDefault(a => a.IsPrimary);
            }
        }
    }
}
