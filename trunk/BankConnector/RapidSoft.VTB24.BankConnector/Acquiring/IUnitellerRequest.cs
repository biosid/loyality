namespace RapidSoft.VTB24.BankConnector.Acquiring
{
    using System.Net.Http;

    public interface IUnitellerRequest
    {
        FormUrlEncodedContent GetFormContent();
    }
}