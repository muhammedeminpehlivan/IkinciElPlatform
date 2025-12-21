using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IkinciElPlatform.Models;

using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IkinciElPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Admin Ana Sayfa
        public IActionResult Index()
        {
            return View();
        }

        // ✅ KATEGORİLER
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(category);
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction("Categories");
        }

        // ✅ TÜM İLANLAR (ADMİN)
        public IActionResult Products()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Purchases) // 🔥 SATIN ALINMIŞ MI BİLGİSİ
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(products);
        }

        // ✅ ADMİN → İLAN SİL
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Products");
        }

        // ✅ ADMİN → AKTİF / PASİF (SATILANI DEĞİŞTİREMEZ)
        public IActionResult ToggleProductStatus(int id)
        {
            var product = _context.Products
                .Include(x => x.Purchases)
                .FirstOrDefault(x => x.Id == id);

            if (product == null)
                return RedirectToAction("Products");

            // ❌ Satın alınmış ürüne admin dokunamaz
            if (product.Purchases != null && product.Purchases.Any())
                return RedirectToAction("Products");

            product.IsActive = !product.IsActive;
            _context.SaveChanges();

            return RedirectToAction("Products");
        }

        // ✅ ADMİN → İSTATİSTİKLER
        public IActionResult Statistics()
        {
            var totalUsers = _context.Users.Count();
            var totalProducts = _context.Products.Count();
            var activeProducts = _context.Products.Count(x => x.IsActive);
            var passiveProducts = _context.Products.Count(x => !x.IsActive);
            var t
            var totalSales = _context.Purchases.Count(); // 🔥 YENİ

            var mostFavoritedProduct = _context.Favorites
                .GroupBy(x => x.ProductId)
                .Select(g => new { ProductId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefault();

            Product? topProduct = null;

            if (mostFavoritedProduct != null)
            {
                topProduct = _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefault(x => x.Id == mostFavoritedProduct.ProductId);
            }

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.ActiveProducts = activeProducts;
            ViewBag.PassiveProducts = passiveProducts;
            ViewBag.TotalFavorites = totalFavorites;
            ViewBag.TotalSales = totalSales;
            ViewBag.TopProduct = topProduct;

            return View();
        }
    }
}
