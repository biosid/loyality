using Vtb24.Site.Services.Models.Exceptions;

namespace Vtb24.Site.Services.MyInfoService.Models.Exceptions
{
    public class MyInfoException : ComponentException
    {
        public MyInfoException(int resultCode, string codeDescription) : base("Мои данные", resultCode, codeDescription)
        {
        }
    }
}