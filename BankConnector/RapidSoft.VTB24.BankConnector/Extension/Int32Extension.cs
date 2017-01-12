namespace RapidSoft.VTB24.BankConnector.Extension
{
    internal static class Int32Extension
    {
        public static bool IsFail(this int val)
        {
            return val != 0;
        }
    }
}