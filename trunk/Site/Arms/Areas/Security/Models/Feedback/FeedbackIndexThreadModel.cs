using System;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminFeedbackServiceEndpoint;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public class FeedbackIndexThreadModel
    {

        private int _page;
        private string _clientId;

        public Guid Id { get; set; }
        
        public FeedbackTypeModel Type { get; set; }

        public bool IsUnanswered { get; set; }

        public string Message { get; set; }

        public string Author { get; set; }

        public bool ShowAuthorLink { get; set; }
        
        public DateTime CreatedTime { get; set; }

        public string LastMessageBy { get; set; }

        public DateTime LastMessageTime { get; set; }

        public int[] SearchMatches { get; set; }

        public string ThreadUrl(Func<Guid, int, string> foo)
        {
            return foo(Id, _page);
        }

        public string AuthorUrl(Func<string, string> foo)
        {
            return foo(_clientId);
        }

        public static FeedbackIndexThreadModel Map(ThreadSearchResult searchResult, int pageSize)
        {
            var model = new FeedbackIndexThreadModel
            {
                _page = (int) Math.Ceiling((double)searchResult.Thread.MessagesCount / pageSize),
                _clientId = searchResult.Thread.ClientId,
                ShowAuthorLink = searchResult.Thread.ClientType == ThreadClientTypes.Client,
                Id = searchResult.Thread.Id,
                Type = MapType(searchResult.Thread.Type),
                IsUnanswered = !searchResult.Thread.IsAnswered,
                Message = searchResult.Thread.TopicMessage.MessageBody,
                Author = searchResult.Thread.ClientFullName,
                CreatedTime = searchResult.Thread.InsertedDate,
                LastMessageBy = searchResult.Thread.LastMessageBy,
                LastMessageTime = searchResult.Thread.LastMessageTime,
            };

            if (searchResult.MessageMatchIndexes != null && searchResult.MessageMatchIndexes.Any())
            {
                model.SearchMatches = searchResult.MessageMatchIndexes.ToArray();
            }

            return model;
        }

        private static FeedbackTypeModel MapType(ThreadTypes type)
        {
            switch (type)
            {
                case ThreadTypes.Issue:
                    return FeedbackTypeModel.Issue;
                case ThreadTypes.Suggestion:
                    return FeedbackTypeModel.Suggestion;
                case ThreadTypes.OrderIncident:
                    return FeedbackTypeModel.OrderIncident;
                default:
                    throw new InvalidOperationException("Неизвестный тип ветки " + type);
            }
        }
    }
}