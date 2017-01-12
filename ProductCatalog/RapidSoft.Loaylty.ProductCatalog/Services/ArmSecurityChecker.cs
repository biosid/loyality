namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.VTB24.ArmSecurity;

    using VTB24.ArmSecurity.Interfaces;

    using ArmPermissions = RapidSoft.VTB24.ArmSecurity.ArmPermissions;

    public class ArmSecurityChecker : ISecurityChecker
    {
        private readonly string[] baseComponentPermissions = new[]
                                                                    {
                                                                        ArmPermissions.ARMProductCatalogLogin
                                                                    };

        private readonly ILog log = LogManager.GetLogger(typeof(ArmSecurityChecker));

        public void CheckPermissions(string accountName, int? partnerId, params string[] permissions)
        {
            if (partnerId.HasValue)
            {
                CheckPermissions(accountName, partnerId.Value, permissions);
            }
            else
            {
                CheckPermissions(accountName, permissions);
            }
        }

        public void CheckPermissions(string accountName, int partnerId, params string[] permissions)
        {
            var inner = baseComponentPermissions.Union(permissions).ToArray();

            var has = ArmSecurity.CheckObjectPermissionsByAccountName(
                accountName, inner, partnerId.ToString(CultureInfo.InvariantCulture));

            if (!has)
            {
                var messFormat = "Не достаточно прав на партнера {0}, необходимые права: {1}.";
                var mess = string.Format(messFormat, partnerId, string.Join(", ", inner));
                log.Error(mess);
                throw new OperationException(ResultCodes.NOT_HAVE_PERMISSION, mess);
            }
        }

        public void CheckPermissions(string accountName, params string[] permissions)
        {
            var inner = baseComponentPermissions.Union(permissions).ToArray();

            var has = ArmSecurity.CheckPermissionsByAccountName(accountName, inner);

            if (!has)
            {
                var messFormat = "Не достаточно прав, необходимые права: {0}.";
                var mess = string.Format(messFormat, string.Join(", ", inner));
                log.Error(mess);
                throw new OperationException(ResultCodes.NOT_HAVE_PERMISSION, mess);
            }
        }
    }
}