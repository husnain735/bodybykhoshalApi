using System.ComponentModel.DataAnnotations;

namespace bodybykhoshalApi.Models.Entities
{
    public class Chats
    {
        [Key]
        public int ChatId { get; set; }
        public string? UserId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Content { get; set; }
    }
}
