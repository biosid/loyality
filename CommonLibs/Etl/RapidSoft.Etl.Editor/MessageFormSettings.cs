using System;
using System.Windows.Forms;

namespace RapidSoft.Etl.Editor
{
    [Serializable]
    public struct MessageFormSettings
    {
        #region Properties

        public string DefaultDisplayText
        {
            get;
            set;
        }

        public bool HideCopyButton
        {
            get;
            set;
        }

        public string CopyButtonText
        {
            get;
            set;
        }

        public bool HideDetailsButton
        {
            get;
            set;
        }

        public string DetailsButtonText
        {
            get;
            set;
        }

        //public bool HideSendMailButton
        //{
        //    get;
        //    set;
        //}

        //public string SendMailButtonText
        //{
        //    get;
        //    set;
        //}

        public string OkButtonText
        {
            get;
            set;
        }

        public string CancelButtonText
        {
            get;
            set;
        }

        public string YesButtonText
        {
            get;
            set;
        }

        public string NoButtonText
        {
            get;
            set;
        }

        public string AbortButtonText
        {
            get;
            set;
        }

        public string RetryButtonText
        {
            get;
            set;
        }

        public string IgnoreButtonText
        {
            get;
            set;
        }

        //public string MailClientName
        //{
        //    get;
        //    set;
        //}

        //public string SendMailMessage
        //{
        //    get;
        //    set;
        //}

        //public bool SkipSendMailMessage
        //{
        //    get;
        //    set;
        //}

        //public string MailSubjectFormat
        //{
        //    get;
        //    set;
        //}

        //public string MailBodyIntroFormat
        //{
        //    get;
        //    set;
        //}

        //public string MailBodyShortIntroFormat
        //{
        //    get;
        //    set;
        //}

        //public bool DoNotAttachScreenshot
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region Methods

        public static MessageFormSettings Merge(MessageFormSettings destination, MessageFormSettings source)
        {
            if (string.IsNullOrEmpty(destination.DefaultDisplayText))
            {
                destination.DefaultDisplayText = source.DefaultDisplayText;
            }

            if (string.IsNullOrEmpty(destination.CopyButtonText))
            {
                destination.CopyButtonText = source.CopyButtonText;
            }

            if (string.IsNullOrEmpty(destination.DetailsButtonText))
            {
                destination.DetailsButtonText = source.DetailsButtonText;
            }

            if (string.IsNullOrEmpty(destination.OkButtonText))
            {
                destination.OkButtonText = source.OkButtonText;
            }

            if (string.IsNullOrEmpty(destination.CancelButtonText))
            {
                destination.CancelButtonText = source.CancelButtonText;
            }

            if (string.IsNullOrEmpty(destination.YesButtonText))
            {
                destination.YesButtonText = source.YesButtonText;
            }

            if (string.IsNullOrEmpty(destination.NoButtonText))
            {
                destination.NoButtonText = source.NoButtonText;
            }

            if (string.IsNullOrEmpty(destination.AbortButtonText))
            {
                destination.AbortButtonText = source.AbortButtonText;
            }

            if (string.IsNullOrEmpty(destination.RetryButtonText))
            {
                destination.RetryButtonText = source.RetryButtonText;
            }

            if (string.IsNullOrEmpty(destination.IgnoreButtonText))
            {
                destination.IgnoreButtonText = source.IgnoreButtonText;
            }

            return destination;
        }

        #endregion
    }
}