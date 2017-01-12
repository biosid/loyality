namespace Vtb24.Site.Services.ClientMessage.Models.Exceptions
{
    public class ThreadSecurityException : ClientMessageServiceException
    {
        public ThreadSecurityException(int resultCode, string resultDescription) : base(resultCode, resultDescription)
        {
        }
    }
}