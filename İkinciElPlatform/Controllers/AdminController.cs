using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IkinciElPlatform.Controllers
{
    [Authorize(Roles = "Admin")]   // ✅ SADECE ADMİN GİRER
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ ADMİN ANA PANEL
        public IActionResult Index()
        {
            ViewBag.UserCount = _context.Users.Count();
            ViewBag.ProductCount = _context.Products.Count();
            ViewBag.CategoryCount = _context.Categories.Count();

            return View();
        }

        // ✅ KATEGORİ LİSTESİ
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // ✅ KATEGORİ EKLE (GET)
        public IActionResult AddCategory()
        {
            return View();
        }

        // ✅ KATEGORİ EKLE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View(category);
        }

        // ✅ KATEGORİ SİL
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        // ======================================================
        // ✅ ✅ ✅  İLAN (ÜRÜN) YÖNETİMİ
        // ======================================================

        // ✅ TÜM İLANLAR (ADMİN)
        public IActionResult Products()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedDate)
                .ToList();

            return View(products);
        }

        // ✅ İLAN AKTİF / PASİF YAP
        public IActionResult ToggleProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.IsActive = !product.IsActive;
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        // ✅ ADMİN İLAN SİL
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Products");
        }
    }
}
