using System.ComponentModel.DataAnnotations;

namespace bodybykhoshalApi.Models.Entities
{
    public class Packages
    {
        [Key]
        public int PackagesId { get; set; }
        public int? TotalNumberOfSessions { get; set; }
        public decimal? PricePerSession { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public string? PackageName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? OrderId { get; set; }
    }
}
