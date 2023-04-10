using Microsoft.AspNetCore.Identity;
using NZWSO.Models;

namespace NZWSO.Data
{
    public class AppUser : IdentityUser

    {
        public List<Product>? Products { get; set; } = new();
        public List<UserProducts>? UserProducts { get; set; }
    }
}