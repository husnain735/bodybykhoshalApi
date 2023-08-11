using System.ComponentModel.DataAnnotations;

namespace bodybykhoshalApi.Models.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Details { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? UserId { get; set; }
    }
}
