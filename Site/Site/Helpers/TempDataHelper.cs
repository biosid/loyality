using System.Web.Mvc;
using Vtb24.Site.Models.Shared;

namespace Vtb24.Site.Helpers
{
    public static class TempDataHelper
    {
        private const string ACTIVE_ADVERTISEMENT_KEY = "ActiveAdvertisement";
        private const string PROPOSE_TO_SET_EMAIL_KEY = "ProposeToSetEmail";

        public static void SetActiveAdvertisement(this TempDataDictionary tempData, AdvertisementModel value)
        {
            tempData[ACTIVE_ADVERTISEMENT_KEY] = value;
        }

        public static bool TryGetActiveAdvertisement(this TempDataDictionary tempData, out AdvertisementModel result)
        {
			result = null;

			object value;
			if (tempData.TryGetValue(ACTIVE_ADVERTISEMENT_KEY, out value))
			{
				result = (AdvertisementModel)value;
				return true;
			}

			return false;
        }

        public static void ProposeToSetEmail(this TempDataDictionary tempData)
        {
            tempData[PROPOSE_TO_SET_EMAIL_KEY] = true;
        }

        public static bool ShowProposalToSetEmail(this TempDataDictionary tempData)
        {
            object value;

            return
                tempData.TryGetValue(PROPOSE_TO_SET_EMAIL_KEY, out value) &&
                value is bool &&
                (bool) value;
        }
    }
}