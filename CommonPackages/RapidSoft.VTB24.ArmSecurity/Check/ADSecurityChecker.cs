namespace RapidSoft.VTB24.ArmSecurity.Check
{
    using System.Globalization;

    using Interfaces;

    public class ADSecurityChecker : ISecurityChecker
    {
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
            var has = ArmSecurity.CheckObjectPermissionsByAccountName(
                accountName, permissions, partnerId.ToString(CultureInfo.InvariantCulture));

            if (!has)
            {
                var messFormat = "Не достаточно прав на партнера {0}, необходимые права: {1}.";
                var mess = string.Format(messFormat, partnerId, string.Join(", ", permissions));
                throw new SecurityCheckException(mess);
            }
        }

        public void CheckPermissions(string accountName, params string[] permissions)
        {
            var has = ArmSecurity.CheckPermissionsByAccountName(accountName, permissions);

            if (!has)
            {
                var messFormat = "Не достаточно прав, необходимые права: {0}.";
                var mess = string.Format(messFormat, string.Join(", ", permissions));
                throw new SecurityCheckException(mess);
            }
        }
    }
}