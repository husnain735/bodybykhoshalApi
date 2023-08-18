using System.ComponentModel.DataAnnotations;

namespace bodybykhoshalApi.Models.Entities
{
    public class ShoppingCart
    {
        [Key]
        public int ShoppingCartId { get; set; }
        public string? UserId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public int? StatusId { get; set; }
        public int? TotalSessions { get; set; }
    }
}
