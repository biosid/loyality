using System.Web;
using Vtb24.Site.Models.Buy;

namespace Vtb24.Site.Models.Shared
{
    public class PostCommandModel
    {
        public enum Commands
        {
            Refresh,
            ReloadCardRegisterFrame,
            ReloadCardSuccessFrame,
            ReloadOnlineOrderFrame,
            ReloadPayAdvanceFrame,
            ShowCardRegistrationError,
            ShowBuyFailed,
            ShowBuyConfirm,
            ShowActivationRequired
        }

        public Commands Command { get; set; }

        public string Query { get; set; }

        public static PostCommandModel Refresh()
        {
            return new PostCommandModel
            {
                Command = Commands.Refresh
            };
        }

        public static PostCommandModel ReloadCardRegisterFrame(string returnUrl)
        {
            return new PostCommandModel
            {
                Command = Commands.ReloadCardRegisterFrame,
                Query = string.Format(RELOAD_CARD_REGISTER_FRAME_QUERY, HttpUtility.UrlEncode(returnUrl))
            };
        }

        public static PostCommandModel ReloadCardSuccessFrame(string returnUrlBase64)
        {
            return new PostCommandModel
            {
                Command = Commands.ReloadCardSuccessFrame,
                Query = string.Format(RELOAD_CARD_SUCCESS_FRAME_QUERY, HttpUtility.UrlEncode(returnUrlBase64))
            };
        }

        public static PostCommandModel ReloadOnlineOrderFrame(OnlineOrderModel model)
        {
            return new PostCommandModel
            {
                Command = Commands.ReloadOnlineOrderFrame,
                Query = string.Format(RELOAD_ONLINE_ORDER_FRAME_QUERY,
                                      model.ShopId, model.OrderId, model.MaxDiscount, model.UserTicket, model.Signature)
            };
        }

        public static PostCommandModel ReloadPayAdvanceFrame(PayAdvanceModel model)
        {
            return new PostCommandModel
            {
                Command = Commands.ReloadPayAdvanceFrame,
                Query = string.Format(RELOAD_PAY_ADVANCE_FRAME_QUERY, model.OrderId, model.OtpToken)
            };
        }

        public static PostCommandModel ShowCardRegistrationError()
        {
            return new PostCommandModel
            {
                Command = Commands.ShowCardRegistrationError
            };
        }

        public static PostCommandModel ShowBuyFailed()
        {
            return new PostCommandModel
            {
                Command = Commands.ShowBuyFailed
            };
        }

        public static PostCommandModel ShowBuyConfirm(ConfirmOrderModel model)
        {
            return new PostCommandModel
            {
                Command = Commands.ShowBuyConfirm,
                Query = string.Format(SHOW_BUY_CONFIRM_QUERY, model.OrderDraftId, model.OtpToken)
            };
        }

        public static PostCommandModel ShowActivationRequired()
        {
            return new PostCommandModel
            {
                Command = Commands.ShowActivationRequired
            };
        }

        private const string RELOAD_CARD_REGISTER_FRAME_QUERY = "?returnurl={0}&refreshdisabled=true";
        private const string RELOAD_CARD_SUCCESS_FRAME_QUERY = "?returnurlbase64={0}&refreshdisabled=true";
        private const string RELOAD_ONLINE_ORDER_FRAME_QUERY = "?shopid={0}&orderid={1}&maxdiscount={2}&userticket={3}&signature={4}&refreshdisabled=true";
        private const string RELOAD_PAY_ADVANCE_FRAME_QUERY = "?orderid={0}&otptoken={1}&refreshdisabled=true";
        private const string SHOW_BUY_CONFIRM_QUERY = "?orderdraftid={0}&otptoken={1}";
    }
}
