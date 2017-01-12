using System;
using System.Configuration;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using RapidSoft.Loyalty.Security;

namespace RapidSoft.Loaylty.PartnersConnector.TestPartner.Controllers
{
    using RapidSoft.Loaylty.PartnersConnector.WsClients;
    using RapidSoft.Loaylty.PartnersConnector.WsClients.PartnerSecurityService;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendNotifyOrderStatus()
        {
            var sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<NotifyOrderStatusMessage xmlns=\"http://tempuri.org/XMLSchema.xsd\">");
            sb.AppendLine("\t<Orders>");
            this.Append(1, sb);
            this.Append(2, sb);
            this.Append(3, sb);
            sb.AppendLine("\t</Orders>");
            sb.AppendLine("</NotifyOrderStatusMessage>");

            var message = sb.ToString();
            this.ViewData["to"] = message;

            var uri = new Uri(ConfigurationManager.AppSettings["NotifyOrderStatusURL"]);
            var certificate = GetCertificate();
            var dispatcher = new TextOverSslMessageDispatcher(certificate);

            try
            {
                var response = dispatcher.Send(uri, message);
                this.ViewData["from"] = response;
            }
            catch (Exception ex)
            {
                this.ViewData["from"] = ex.ToString();
            }

            return this.View("Index");
        }

        private void Append(int index, StringBuilder sb)
        {
            var req = this.Request;
            var isOn = req.Form["Order" + index] == "on";
            if (isOn)
            {
                sb.AppendLine("\t\t<Order>");
                sb.Append("\t\t\t<InternalOrderId>").Append(req.Form["OrderId" + index]).AppendLine("</InternalOrderId>");
                sb.Append("\t\t\t<StatusCode>").Append(req.Form["StatusCode" + index]).AppendLine("</StatusCode>");
                sb.Append("\t\t\t<StatusDateTime>").Append(req.Form["StatusDateTime" + index]).AppendLine("</StatusDateTime>");
                var t1 = req.Form["StatusReason" + index];
                if (!string.IsNullOrWhiteSpace(t1))
                {
                    sb.Append("\t\t\t<StatusReason>").Append(t1).AppendLine("</StatusReason>");
                }

                var t2 = req.Form["InternalStatusCode" + index];
                if (!string.IsNullOrWhiteSpace(t1))
                {
                    sb.Append("\t\t\t<InternalStatusCode>").Append(t2).AppendLine("</InternalStatusCode>");
                }

                sb.AppendLine("\t\t</Order>");
            }
        }

        private X509Certificate2 GetCertificate()
        {
            var thumbprint = ConfigurationManager.AppSettings["TestPartnerThumbprint"];
            return new StoreCertificateProvider().GetByThumbprint(thumbprint);
        }

        [ValidateInput(false)]
        public ActionResult GenerateSignature()
        {
            var input = this.Request["Input"];

            var retVal =
                WebClientCaller.CallService<PartnerSecurityServiceClient, SignatureResult>(
                    cl => cl.CreateBankSignature(input));

            this.ViewData["Signature"] = retVal.Signature;

            return this.View("Index");
        }
    }
}
