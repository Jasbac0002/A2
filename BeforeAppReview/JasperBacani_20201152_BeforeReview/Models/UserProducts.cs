using NZWSO.Data;

namespace NZWSO.Models
{
    public class UserProducts
    {
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
