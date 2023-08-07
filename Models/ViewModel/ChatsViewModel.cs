namespace bodybykhoshalApi.Models.ViewModel
{
    public class ChatsViewModel
    {
        public int ChatId { get; set; }
        public string? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Content { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? RoleId { get; set; }
    }
}
