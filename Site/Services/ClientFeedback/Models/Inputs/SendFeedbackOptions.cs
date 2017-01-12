namespace Vtb24.Site.Services.ClientFeedback.Models.Inputs
{
    public class SendFeedbackOptions
    {
        public string Title { get; set; }
        public FeedbackType Type { get; set; }

        public string ClientFullName { get; set; }

        public string ClientEmail { get; set; }

        public string Text { get; set; }
        public string MetaData { get; set; }
    }
}