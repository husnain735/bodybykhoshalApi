namespace bodybykhoshalApi.Models.ViewModel
{
  public class ChatsViewModel
  {
    public int ChatId { get; set; }
    public string? SenderOne { get; set; }
    public string? SenderTwo { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Content { get; set; }
    public int? RoleId { get; set; }
    public string? SenderName { get; set; }
    public int? ChatType { get; set; }
  }
}
