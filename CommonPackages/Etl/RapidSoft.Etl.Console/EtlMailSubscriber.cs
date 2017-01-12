using System;

namespace RapidSoft.Etl.Console
{
    [Serializable]
    public sealed class EtlMailSubscriber
    {
        public string Email
        {
            get;
            set;
        }
    }
}