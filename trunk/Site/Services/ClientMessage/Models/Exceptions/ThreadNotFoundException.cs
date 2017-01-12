namespace Vtb24.Site.Services.ClientMessage.Models.Exceptions
{
    public class ThreadNotFoundException : ClientMessageServiceException
    {
        public ThreadNotFoundException(int resultCode, string resultDescription) : base(resultCode, resultDescription)
        {
        }
    }
}