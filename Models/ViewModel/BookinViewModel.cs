namespace bodybykhoshalApi.Models.ViewModel
{
    public class BookinViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? Details { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserId { get; set; }
    }
}
