namespace Rapidsoft.Loyalty.NotificationSystem.API.InputParameters
{
    using System;
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class AdminGetThreadsParameters
    {
        [DataMember]
        public string UserId
        {
            get;
            set;
        }

        [DataMember]
        public string ClientId
        {
            get;
            set;
        }

        [DataMember]
        public AnsweredFilters Filter
        {
            get;
            set;
        }

        [DataMember]
        public FeedbackTypes? FeedbackType
        {
            get;
            set;
        }

        [DataMember]
        public int CountToSkip { get; set; }

        [DataMember(IsRequired = true)]
        public int CountToTake { get; set; }

        [DataMember]
        public DateTime? MaxDate
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? MinDate
        {
            get;
            set;
        }

        [DataMember]
        public ThreadClientTypes? ClientType
        {
            get;
            set;
        }
        
        [DataMember]
        public string ClientEmail
        {
            get;
            set;
        }

        [DataMember]
        public string SearchTerm
        {
            get;
            set;
        }

        [DataMember]
        public string OperatorLogin
        {
            get;
            set;
        }
    }
}