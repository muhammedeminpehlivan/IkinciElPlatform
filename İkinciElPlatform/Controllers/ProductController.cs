using IkinciElPlatform.Data;
using IkinciElPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace IkinciElPlatform.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ TÜM ÜRÜNLER + FİLTRE + SIRALAMA
        public IActionResult Index(string search, int? categoryId, decimal? minPrice, decimal? maxPrice, string sort)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)  // sadece aktif ilanlar
                .AsQueryable();

            // ✅ Arama
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Title.Contains(search));
            }

            // ✅ Kategori filtresi
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            // ✅ Min fiyat
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            // ✅ Max fiyat  (BURADA HATA VARDI, minPrice yazıyordu)
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            // ✅ SIRALAMA
            switch (sort)
            {
                case "priceAsc":
                    products = products.OrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    products = products.OrderByDescending(p => p.Price);
                    break;

                case "newest":
                    products = products.OrderByDescending(p => p.CreatedDate);
                    break;

                case "oldest":
                    products = products.OrderBy(p => p.CreatedDate);
                    break;
            }

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Sort = sort;

            return View(products.ToList());
        }

        // ✅ ÜRÜN DETAY
        public IActionResult Detail(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id && p.IsActive);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // ✅ DÜZENLEME SAYFASI (GET)
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            // sadece kendi ürününü düzenlesin
            if (product.SellerName != User.Identity.Name)
                return RedirectToAction("MyProducts", "Profile");

            ViewBag.CategoryList = new SelectList(
                _context.Categories.ToList(),
                "Id",
                "Name",
                product.CategoryId
            );

            return View(product);
        }

        // ✅ DÜZENLEME KAYIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var dbProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

            if (dbProduct == null)
                return NotFound();

            if (dbProduct.SellerName != User.Identity.Name)
                return RedirectToAction("MyProducts", "Profile");

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Brand = product.Brand;
            dbProduct.Condition = product.Condition;
            dbProduct.Location = product.Location;
            dbProduct.IsNegotiable = product.IsNegotiable;

            _context.SaveChanges();

            return RedirectToAction("MyProducts", "Profile");
        }

        // ✅ SİLME ONAY SAYFASI (GET)
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.SellerName) &&
                product.SellerName != User.Identity.Name)
            {
                return RedirectToAction("MyProducts", "Profile");
            }

            return View(product);
        }

        // ✅ SİLME ONAY (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.SellerName) &&
                product.SellerName != User.Identity.Name)
            {
                return RedirectToAction("MyProducts", "Profile");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("MyProducts", "Profile");
        }
        [HttpPost]
        [Authorize]
        public IActionResult Buy(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null || !product.IsActive)
                return RedirectToAction("Index", "Home");

            var buyerId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            // ❌ Satıcı kendi ürününü satın alamaz (güzel dokunuş)
            if (product.UserId == buyerId)
            {
                TempData["BuyMessage"] = "Kendi ürününü satın alamazsın.";
                return RedirectToAction("Detail", new { id });
            }

            var purchase = new Purchase
            {
                ProductId = product.Id,
                BuyerId = buyerId,
                SellerId = product.UserId,
                PurchaseDate = DateTime.Now,
                Status = "Satıldı"
            };

            product.IsActive = false;

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            TempData["BuyMessage"] = "🛒 Ürün başarıyla satın alındı.";

            // ✅ BOŞ SAYFA YOK → Satın Aldıklarım
            return RedirectToAction("MyPurchases", "Profile");
        }

    }
}
