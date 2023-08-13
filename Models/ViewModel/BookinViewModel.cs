namespace bodybykhoshalApi.Models.ViewModel
{
  public class BookinViewModel
  {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Details { get; set; }
    public int? StatusId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public bool? IsDeleted { get; set; }
    public string? UserId { get; set; }
  }
}
