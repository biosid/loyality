using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;
using Vtb24.Arms.Infrastructure;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public class FeedbackIndexFiltersModel
    {
        public FeedbackIndexFiltersModel()
        {
            page = 1;
        }

        // ReSharper disable once InconsistentNaming
        public FeedbackTypeFilterModel? type { get; set; }

        [StringLength(150), AllowHtml]
        // ReSharper disable once InconsistentNaming
        public string keywords { get; set; }

        [Mask("+7 (999) 999-9999")]
        // ReSharper disable once InconsistentNaming
        public string phone { get; set; }

        [StringLength(150)]
        // ReSharper disable once InconsistentNaming
        public string email { get; set; }

        [StringLength(150)]
        // ReSharper disable once InconsistentNaming
        public string oplogin { get; set; }

        // ReSharper disable once InconsistentNaming
        public DateTime? from { get; set; }

        // ReSharper disable once InconsistentNaming
        public DateTime? to { get; set; }

        // ReSharper disable once InconsistentNaming
        public int page { get; set; }

        public FeedbackTypes? MapThreadType()
        {
            switch (type)
            {
                case FeedbackTypeFilterModel.all:
                    return null;
                case FeedbackTypeFilterModel.issue:
                    return FeedbackTypes.Issue;
                case FeedbackTypeFilterModel.suggestion:
                    return FeedbackTypes.Suggestion;
                case FeedbackTypeFilterModel.orderincident:
                    return FeedbackTypes.OrderIncident;
                default:
                    return null;
            }
        }
    }
}