using NZWSO.Data;
using System.ComponentModel.DataAnnotations;

namespace NZWSO.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public List<AppUser>? Users { get; set; } = new();
        public List<UserProducts>? UserProducts { get; set; }

        public DateTime? CreatedAtDate { get; set; } = default(DateTime?);


    }
}
