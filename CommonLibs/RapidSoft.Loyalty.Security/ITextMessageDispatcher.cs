using System;

namespace RapidSoft.Loyalty.Security
{
    public interface ITextMessageDispatcher : IMessageDispatcher<string, Uri, string>
    {
    }
}