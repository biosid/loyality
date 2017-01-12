using System;
using System.ComponentModel;

namespace RapidSoft.Etl.Logging
{
	public static class EtlStatuses
	{
        public static string ToString(EtlStatus status)
        {
            switch (status)
            {
                case EtlStatus.Waiting:
                    return Properties.Resources.Waiting;
                case EtlStatus.Started:
                    return Properties.Resources.Started;
                case EtlStatus.Succeeded:
                    return Properties.Resources.Succeeded;
                case EtlStatus.FinishedWithLosses:
                    return Properties.Resources.FinishedWithLosses;
                case EtlStatus.FinishedWithWarnings:
                    return Properties.Resources.FinishedWithWarnings;
                case EtlStatus.Failed:
                    return Properties.Resources.Failed;
				case EtlStatus.FinishedWithSessionEnd:
					return Properties.Resources.FinishedWithSessionEnd;
				default:
                    return Properties.Resources.UnknownStatus;
            }
        }

        public static EtlStatus GetPriorityStatus(EtlStatus left, EtlStatus right)
        {
            var cmp = Compare(left, right);

            if (cmp == 0)
            {
                return left;
            }
            else if (cmp > 0)
            {
                return left;
            }
            else
            {
                return right;
            }
        }

        private static int Compare(EtlStatus left, EtlStatus right)
        {
            var intleft = (int)left;
            var intright = (int)right;

            if (intleft == intright)
            {
                return 0;
            }
            else if (left > right)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
	}
}