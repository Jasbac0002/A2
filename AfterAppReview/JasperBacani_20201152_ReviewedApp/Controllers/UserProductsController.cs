using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NZWSO.Data;
using NZWSO.Models;

namespace NZWSO.Controllers
{
    [Authorize]
    public class UserProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserProductsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserProducts
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(_context.UserProducts.Include(u => u.Product).Include(u => u.User));
            }
            else
            {
                return View(_context.UserProducts.Include(x => x.Product).Include(u => u.User).Where(x => x.UserId == _userManager.GetUserId(User)));
            }
        }

        public async Task<IActionResult> Delete(int productId, string userId)
        {
            var exists = _context.UserProducts.Include(x => x.User).Include(x => x.Product).FirstOrDefault(x => x.ProductId == productId && x.UserId == userId);

            if (exists == null) { return RedirectToAction(nameof(Index)); }

            _context.UserProducts.Remove(exists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProductsExists(string id)
        {
            return (_context.UserProducts?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
