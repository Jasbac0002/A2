using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NZWSO.Data;
using NZWSO.Models;

namespace NZWSO.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public async Task<IActionResult> Buy(int id)
        {
            if (id == null || id == 0) RedirectToAction(nameof(Index));
            try
            {
                var userId = _userManager.GetUserId(User);
                UserProducts userProducts = new UserProducts();
                userProducts.ProductId = id;
                userProducts.UserId = userId;

                var exists = _context.UserProducts.Any(x => x.UserId == userId && x.ProductId == id);

                if(exists) return RedirectToAction("Index", "UserProducts");

                _context.UserProducts.Add(userProducts);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "UserProducts");
            }
            catch
            {
                return BadRequest();
            }                      
        }

        public async Task<IActionResult> Index(string sortOrder, bool descending = false)
        {
            if( _context.Products != null)
            {
                View(await _context.Products.ToListAsync());
                    Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
                        

            var items = _context.Products.ToList();

            switch (sortOrder)
            {
                case "Name":
                    items = descending ? items.OrderByDescending(i => i.Name).ToList() : items.OrderBy(i => i.Name).ToList();
                    break;
                case "Date":
                    items = descending ? items.OrderByDescending(i => i.CreatedAtDate).ToList() : items.OrderBy(i => i.CreatedAtDate).ToList();
                    break;
                default:
                    items = items.OrderBy(i => i.Id).ToList();
                    break;
            }

            ViewBag.SortOrder = sortOrder;
            ViewBag.Descending = descending;

            return View(items);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageURL,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageURL,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //// GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
