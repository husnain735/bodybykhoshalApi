namespace bodybykhoshalApi.Models.ViewModel
{
    public class HttpRequest
    {
        public class SaveChatRequestHandler
        {
            public string? UserId { get; set; }
            public DateTime? Timestamp { get; set; }
            public string? Content { get; set; }

        }
    }
}
