using System;

namespace RapidSoft.Etl.Logging
{
	public static class EtlMessageTypes
	{
		public static string ToString(EtlMessageType messageType)
		{
			switch (messageType)
            {
                case EtlMessageType.SessionStart:
                    return Properties.Resources.SessionStart;
                case EtlMessageType.SessionEnd:
                    return Properties.Resources.SessionEnd;
                case EtlMessageType.StepStart:
                    return Properties.Resources.StepStart;
                case EtlMessageType.StepEnd:
                    return Properties.Resources.StepEnd;
                case EtlMessageType.Error:
                    return Properties.Resources.Error;
                case EtlMessageType.Information:
                    return Properties.Resources.Information;
                case EtlMessageType.Debug:
                    return Properties.Resources.Debug;
                case EtlMessageType.Warning:
                    return Properties.Resources.Warning;
                default:
                    return Properties.Resources.UnknownMessageType;
            } 
		}
	}
}