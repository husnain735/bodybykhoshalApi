namespace bodybykhoshalApi.Models.ViewModel
{
    public class HttpRequest
    {
        public class SaveChatRequestHandler
        {
            public string? SenderOne { get; set; }
            public string? SenderTwo { get; set; }
            public DateTime? Timestamp { get; set; }
            public string? Content { get; set; }
            public int? RoleId { get; set; }
            public string? SenderName { get; set; }
        }
        public class GetAdminChatWithCustomerRequestHandler
        {
            public string? SenderOne { get; set; }
            public string? SenderTwo { get; set; }
        }
    }
}
