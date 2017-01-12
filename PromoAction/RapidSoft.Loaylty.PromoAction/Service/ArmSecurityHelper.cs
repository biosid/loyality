using System.Linq;
using RapidSoft.Loaylty.Logging;

namespace RapidSoft.Loaylty.PromoAction.Service
{
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.VTB24.ArmSecurity;

    public static class ArmSecurityHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArmSecurityHelper));

        private static readonly string[] BaseComponentPermissions = new[]
                                                               {
                                                                   ArmPermissions.ARMPromoActionLogin,
                                                                   ArmPermissions.Promo
                                                               };

        public static void CheckPermissions(string accountName, params string[] permissions)
        {
            var inner = BaseComponentPermissions.Union(permissions).ToArray();

            var has = ArmSecurity.CheckPermissionsByAccountName(accountName, inner);

            if (!has)
            {
                var mess = "Не достаточно прав, необходимые права: " + string.Join(", ", inner);
                Log.Error(mess);
                throw new OperationException(ResultCodes.NOT_HAVE_PERMISSION, mess);
            }
        }
    }
}