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
      public bool? IsRead { get; set; }
      public int? ChatType { get; set; }
    }
    public class GetAdminChatWithCustomerRequestHandler
    {
      public string? SenderOne { get; set; }
      public string? SenderTwo { get; set; }
    }
    public class addSessionRequestHandler
    {
      public int? ShoppingCartId { get; set; }
      public int SessionCount { get; set; }
      public string? UserId { get; set; }
    }
  }
}
