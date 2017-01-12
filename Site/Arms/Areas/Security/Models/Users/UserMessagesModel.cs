namespace Vtb24.Arms.Security.Models.Users
{
    public class UserMessagesModel
    {
        public string Login { get; set; }

        public UserMessageModel[] Messages { get; set; }

        public int Page { get; set; }

        public int TotalPages { get; set; }
    }
}
