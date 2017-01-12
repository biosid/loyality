namespace Vtb24.Site.Security
{
    public interface ISmsService
    {
        void Send(string phone, string message);
    }
}